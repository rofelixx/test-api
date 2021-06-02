using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Segfy.Schedule.Infra.Operations;
using Segfy.Schedule.Model.Entities;
using Segfy.Schedule.Model.Filters;
using Segfy.Schedule.Model.Pagination;

namespace Segfy.Schedule.Infra.Repositories.Base
{
    public class TableRepository<T> : ITableRepository<T, Model.Filters.Filter> where T : BaseEntity
    {
        protected readonly IDynamoBDOperations<T> _context;

        public TableRepository(IDynamoBDOperations<T> context)
        {
            _context = context;
        }

        public Task<DynamoDBPagedRequest<T>> All(int limit = 10, string paginationToken = "")
        {
            var parameters = new ScanParameters()
            {
                PaginationToken = paginationToken,
                PerPage = limit,
            };
            return _context.ScanAsync(parameters);
        }

        public async Task<T> Add(T entity)
        {
            var added = await HydrateEntityForCreation(entity);
            await _context.SaveAsync(added);
            return added;
        }

        public async Task<IEnumerable<T>> Add(IEnumerable<T> entities)
        {
            var dummies = new List<T>();
            foreach (var item in entities)
            {
                dummies.Add(await HydrateEntityForCreation(item));
            }

            await _context.SaveAsync(dummies);
            return dummies;
        }

        public Task Remove(Guid hashid, Guid sortid)
        {
            return _context.DeleteAsync(hashid, sortid);
        }

        public Task<T> Single(Guid hashid, Guid sortid)
        {
            return _context.LoadAsync(hashid, sortid);
        }

        public async Task<T> Update(T entity)
        {
            var item = await HydratateEntityForUpdate(entity);

            await _context.SaveAsync(item);
            return item;
        }

        public Task<DynamoDBPagedRequest<T>> Find(Model.Filters.Filter searchReq, int limit = 10, string paginationToken = "")
        {
            return Find(null, searchReq, limit, paginationToken);
        }

        public Task<DynamoDBPagedRequest<T>> Find(string indexName, Model.Filters.Filter searchReq, int limit = 10, string paginationToken = "")
        {
            var parameters = new ScanParameters()
            {
                Filters = new List<Model.Filters.Filter>() { searchReq },
                IndexName = indexName,
                PerPage = limit,
                PaginationToken = paginationToken
            };
            return _context.ScanAsync(parameters);
        }

        public Task<DynamoDBPagedRequest<T>> Query(Guid hashKey, Guid lastRangeKey, int limit = 10, IList<Filter> filters = null)
        {
            var parameters = new QueryParameters()
            {
                PerPage = limit,
                HashKey = hashKey,
                LastRangeKey = lastRangeKey,
                Filters = filters
            };
            return _context.QueryAsync(parameters);
        }

        public Task<DynamoDBPagedRequest<T>> Query(Guid hashKey, int limit = 10)
        {
            return Query(hashKey, Guid.Empty, limit);
        }

        protected virtual Task<T> HydrateEntityForCreation(T entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            return Task.FromResult(entity);
        }

        protected virtual Task<T> HydratateEntityForUpdate(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            return Task.FromResult(entity);
        }

    }
}