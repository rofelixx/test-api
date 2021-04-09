using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Segfy.Schedule.Infra.Handles;
using Segfy.Schedule.Model.Entities;
using Segfy.Schedule.Model.Pagination;

namespace Segfy.Schedule.Infra.Operations
{
    /// O código desta classe não é testável pois a biblioteca da AWS
    /// não usa interfaces para suas operações, além de "esconder" os
    /// os construtores dos tipos concretos utilizados. Portanto, essa
    /// classe de operações do DynamoDB foi criada para conseguirmos 
    /// testar os códigos que utilizam operações dessa lib.
    public class DynamoBDOperations<T> : IDynamoBDOperations<T> where T : BaseEntity
    {
        protected readonly IDynamoDBContext _context;
        protected readonly IDynamoBDFilterHandles _filterHandle;

        public DynamoBDOperations(IDynamoDBContext context, IDynamoBDFilterHandles filterHandle)
        {
            _context = context;
            _filterHandle = filterHandle;
        }

        public Task SaveAsync(T entity)
        {
            return _context.SaveAsync(entity);
        }

        public Task SaveAsync(IEnumerable<T> entity)
        {
            var batch = _context.CreateBatchWrite<T>();
            batch.AddPutItems(entity);
            return batch.ExecuteAsync();
        }

        public Task DeleteAsync(Guid hashid, Guid sortid)
        {
            return _context.DeleteAsync<T>(hashid, sortid);
        }

        public Task<T> LoadAsync(Guid hashid, Guid sortid)
        {
            return _context.LoadAsync<T>(hashid, sortid);
        }

        public async Task<DynamoDBPagedRequest<T>> ScanAsync(ScanParameters parameters)
        {
            var table = _context.GetTargetTable<T>();

            ScanFilter scanFilter = new ScanFilter();
            if (parameters.Filters != null)
            {
                foreach (var item in parameters.Filters)
                {
                    if (string.IsNullOrEmpty(item.Field)) continue;
                    scanFilter.AddCondition(item.Field, ScanOperator.Equal, item.Value);
                }
            }

            var scanOps = new ScanOperationConfig()
            {
                Limit = parameters.PerPage,
                Filter = scanFilter,
            };

            if (!string.IsNullOrWhiteSpace(parameters.PaginationToken))
            {
                scanOps.PaginationToken = parameters.PaginationToken;
            }

            if (!string.IsNullOrWhiteSpace(parameters.IndexName))
            {
                scanOps.IndexName = parameters.IndexName;
            }

            var results = table.Scan(scanOps);
            List<Document> data = await results.GetNextSetAsync();

            var items = _context.FromDocuments<T>(data);

            return new DynamoDBPagedRequest<T>
            {
                PaginationToken = results.PaginationToken,
                Items = items.ToList(),
                Segment = results.Segment,
                TotalSegments = results.TotalSegments,
                IsDone = results.IsDone,
            };
        }

        public async Task<DynamoDBPagedRequest<T>> QueryAsync(QueryParameters parameters)
        {
            var table = _context.GetTargetTable<T>();

            var filter = new QueryFilter();
            filter.AddCondition("subscription_id", QueryOperator.Equal, parameters.HashKey);
            if (parameters.LastRangeKey != Guid.Empty)
            {
                filter.AddCondition("id", QueryOperator.GreaterThan, parameters.LastRangeKey);
            }

            _filterHandle.Apply(filter, parameters.Filters);

            if (parameters.Filters != null && parameters.Filters.Any())
            {
                foreach (Model.Filters.Filter userFilter in parameters.Filters)
                {
                    filter.AddCondition(userFilter.Field, QueryOperator.Equal, userFilter.Value);
                }
            }

            var config = new QueryOperationConfig() { Filter = filter, Limit = 10 };
            if (parameters.PerPage > 0)
            {
                config.Limit = parameters.PerPage;
            }

            var results = table.Query(config);
            List<Document> data = await results.GetNextSetAsync();

            var items = _context.FromDocuments<T>(data);
            return new DynamoDBPagedRequest<T>
            {
                PaginationToken = results.PaginationToken,
                Items = items,
                Segment = results.Segment,
                TotalSegments = results.TotalSegments,
                IsDone = results.IsDone,
                LastEvaluatedKey = results.NextKey,
            };
        }
    }
}