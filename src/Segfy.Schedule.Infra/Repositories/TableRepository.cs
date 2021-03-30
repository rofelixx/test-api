using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Segfy.Schedule.Model.Entities;
using Segfy.Schedule.Model.Pagination;

namespace Segfy.Schedule.Infra.Repositories
{
    public abstract class TableRepository<T, VM, Search> : ITableRepository<T, VM, Search> where T : BaseEntity
    {
        protected readonly DynamoDBContext _context;

        public TableRepository(IAmazonDynamoDB client)
        {
            _context = new DynamoDBContext(client);
        }

        public abstract Task<T> Add(VM entity);
        public abstract Task<IEnumerable<T>> Add(IEnumerable<VM> entities);
        public abstract Task<DynamoDBPagedRequest<T>> All(string paginationToken = "");
        public abstract Task<DynamoDBPagedRequest<T>> Find(Search searchReq, string paginationToken = "");
        public abstract Task Remove(Guid hashid, Guid sortid);
        public abstract Task<T> Single(Guid hashid, Guid sortid);
        public abstract Task<T> Update(Guid hashid, Guid sortid, VM entity);
    }
}