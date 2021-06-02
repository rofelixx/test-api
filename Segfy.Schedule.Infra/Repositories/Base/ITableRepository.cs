using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Segfy.Schedule.Model.Entities;
using Segfy.Schedule.Model.Filters;
using Segfy.Schedule.Model.Pagination;

namespace Segfy.Schedule.Infra.Repositories.Base
{
    public interface ITableRepository<T, Search> where T : BaseEntity
    {
        Task<T> Single(Guid hashid, Guid sortid);
        Task<DynamoDBPagedRequest<T>> All(int limit = 10, string paginationToken = "");
        Task<DynamoDBPagedRequest<T>> Query(Guid hashKey, int limit = 10);
        Task<DynamoDBPagedRequest<T>> Query(Guid hashKey, Guid nextRangeKey, int limit = 10, IList<Filter> filters = null);
        Task<DynamoDBPagedRequest<T>> Find(Search searchReq, int limit = 10, string paginationToken = "");
        Task<DynamoDBPagedRequest<T>> Find(string indexName, Search searchReq, int limit = 10, string paginationToken = "");
        Task<T> Add(T entity);
        Task<IEnumerable<T>> Add(IEnumerable<T> entities);
        Task Remove(Guid hashid, Guid sortid);
        Task<T> Update(T entity);
    }
}