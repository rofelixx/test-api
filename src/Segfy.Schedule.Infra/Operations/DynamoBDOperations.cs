using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
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

        public DynamoBDOperations(IDynamoDBContext context)
        {
            _context = context;
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
                Items = items,
                Segment = results.Segment,
                TotalSegments = results.TotalSegments,
                IsDone = results.IsDone,
            };
        }
    }
}