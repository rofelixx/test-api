using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Segfy.Schedule.Model.Entities;
using Segfy.Schedule.Model.Pagination;

namespace Segfy.Schedule.Infra.Repositories
{
    public abstract class TableRepository<T, VM> : ITableRepository<T, VM, Model.Filters.Filter> where T : BaseEntity
    {
        protected readonly DynamoDBContext _context;

        public TableRepository(IAmazonDynamoDB client)
        {
            _context = new DynamoDBContext(client);
        }

        protected abstract Task<T> HydrateEntityForCreation(VM viewModel);
        protected abstract Task<T> HydratateEntityForUpdate(Guid hashid, Guid sortid, VM entity);

        public async Task<T> Add(VM entity)
        {
            var added = await HydrateEntityForCreation(entity);
            await _context.SaveAsync<T>(added);
            return added;
        }

        public async Task<IEnumerable<T>> Add(IEnumerable<VM> entities)
        {
            var dummies = new List<T>();
            foreach (var item in entities)
            {
                dummies.Add(await HydrateEntityForCreation(item));
            }

            var batch = _context.CreateBatchWrite<T>();
            batch.AddPutItems(dummies);
            await batch.ExecuteAsync();
            return dummies;
        }

        public async Task<DynamoDBPagedRequest<T>> All(string paginationToken = "")
        {
            var table = _context.GetTargetTable<T>();

            var scanOps = new ScanOperationConfig() { Limit = 25 };
            if (!string.IsNullOrEmpty(paginationToken))
            {
                scanOps.PaginationToken = paginationToken;
            }

            // returns the set of Document objects
            // for the supplied ScanOptions
            var results = table.Scan(scanOps);
            List<Document> data = await results.GetNextSetAsync();

            // transform the generic Document objects
            // into our Entity Model
            var items = _context.FromDocuments<T>(data);

            // Pass the PaginationToken
            // if available from the Results
            // along with the Result set
            return new DynamoDBPagedRequest<T>
            {
                PaginationToken = results.PaginationToken,
                Items = items,
                Segment = results.Segment,
                TotalSegments = results.TotalSegments,
                IsDone = results.IsDone,
            };
        }

        public Task<DynamoDBPagedRequest<T>> Find(Model.Filters.Filter searchReq, string paginationToken = "")
        {
            return Find(null, searchReq, paginationToken);
        }

        public Task Remove(Guid hashid, Guid sortid)
        {
            return _context.DeleteAsync<T>(hashid, sortid);
        }

        public Task<T> Single(Guid hashid, Guid sortid)
        {
            return _context.LoadAsync<T>(hashid, sortid);
        }

        public async Task<T> Update(Guid hashid, Guid sortid, VM entity)
        {
            var item = await HydratateEntityForUpdate(hashid, sortid, entity);

            await _context.SaveAsync<T>(item);
            return item;
        }

        public async Task<DynamoDBPagedRequest<T>> Find(string indexName, Model.Filters.Filter searchReq, string paginationToken = "")
        {
            var table = _context.GetTargetTable<T>();

            ScanFilter scanFilter = new ScanFilter();
            if (!string.IsNullOrEmpty(searchReq.Field))
                scanFilter.AddCondition(searchReq.Field, ScanOperator.Equal, searchReq.Value);

            var scanOps = new ScanOperationConfig() { Limit = 20, Filter = scanFilter };
            if (!string.IsNullOrWhiteSpace(indexName))
            {
                scanOps.IndexName = indexName;
            }

            if (!string.IsNullOrEmpty(paginationToken))
            {
                scanOps.PaginationToken = paginationToken;
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