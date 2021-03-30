using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Segfy.Schedule.Model.Entities;
using Segfy.Schedule.Model.Filters;
using Segfy.Schedule.Model.ViewModels;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Segfy.Schedule.Model.Pagination;
using System.Linq;

namespace Segfy.Schedule.Infra.Repositories
{
    public class DummyTableRepository : TableRepository<DummyTable, DummyTableViewModel, Model.Filters.Filter>
    {
        public DummyTableRepository(IAmazonDynamoDB client) : base(client)
        {
        }

        public override async Task<DummyTable> Add(DummyTableViewModel entity)
        {
            var dummy = GetDummyTable(entity);
            await _context.SaveAsync<DummyTable>(dummy);
            return dummy;
        }

        public override async Task<IEnumerable<DummyTable>> Add(IEnumerable<DummyTableViewModel> entities)
        {
            var dummies = new List<DummyTable>();
            foreach (var item in entities)
            {
                dummies.Add(GetDummyTable(item));
            }
            
            var batch = _context.CreateBatchWrite<DummyTable>();
            batch.AddPutItems(dummies);
            await batch.ExecuteAsync();
            return dummies;
        }

        public override async Task<DynamoDBPagedRequest<DummyTable>> All(string paginationToken = "")
        {
            // Get the Table ref from the Model
            var table = _context.GetTargetTable<DummyTable>();

            // If there's a PaginationToken
            // Use it in the Scan options
            // to fetch the next set
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
            var items = _context.FromDocuments<DummyTable>(data);

            // Pass the PaginationToken
            // if available from the Results
            // along with the Result set
            return new DynamoDBPagedRequest<DummyTable>
            {
                PaginationToken = results.PaginationToken,
                Items = items,
                Segment = results.Segment,
                TotalSegments = results.TotalSegments,
                IsDone = results.IsDone,
            };
        }

        public override async Task<DynamoDBPagedRequest<DummyTable>> Find(Model.Filters.Filter searchReq, string paginationToken = "")
        {
            var table = _context.GetTargetTable<DummyTable>();

            ScanFilter scanFilter = new ScanFilter();
            if (!string.IsNullOrEmpty(searchReq.Field))
                scanFilter.AddCondition(searchReq.Field, ScanOperator.Equal, searchReq.Value);

            var scanOps = new ScanOperationConfig() { Limit = 25, Filter = scanFilter };
            if (!string.IsNullOrEmpty(paginationToken))
            {
                scanOps.PaginationToken = paginationToken;
            }

            var results = table.Scan(scanOps);
            List<Document> data = await results.GetNextSetAsync();
            var items = _context.FromDocuments<DummyTable>(data);

            return new DynamoDBPagedRequest<DummyTable>
            {
                PaginationToken = results.PaginationToken,
                Items = items,
                Segment = results.Segment,
                TotalSegments = results.TotalSegments,
                IsDone = results.IsDone,
            };
        }

        public override Task Remove(Guid hashid, Guid sortid)
        {
            return _context.DeleteAsync<DummyTable>(hashid, sortid);
        }

        public override Task<DummyTable> Single(Guid hashid, Guid sortid)
        {
            return _context.LoadAsync<DummyTable>(hashid, sortid);
        }

        public override async Task<DummyTable> Update(Guid hashid, Guid sortid, DummyTableViewModel entity)
        {
            var item = await Single(hashid, sortid);
            item.DummyIndex = entity.DummyIndex;
            item.DummyInteger = entity.DummyInteger;
            item.Text = entity.Text;

            await _context.SaveAsync<DummyTable>(item);
            return item;
        }

        private DummyTable GetDummyTable(DummyTableViewModel entity)
        {
            return new DummyTable()
            {
                Id = Guid.NewGuid(),
                SubscriptionId = entity.SubscriptionId,
                DummyIndex = entity.DummyIndex,
                DummyInteger = entity.DummyInteger,
                Text = entity.Text,
            };
        }
    }
}