using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Segfy.Schedule.Model.Entities;
using Segfy.Schedule.Model.Pagination;

namespace Segfy.Schedule.Infra.Repositories
{
    public interface ITableRepository<T, VM, Search> where T : BaseEntity
    {
        Task<T> Single(Guid hashid, Guid sortid);
        Task<DynamoDBPagedRequest<T>> All(string paginationToken = "");
        Task<DynamoDBPagedRequest<T>> Find(Search searchReq, string paginationToken = "");
        Task<DynamoDBPagedRequest<T>> Find(string indexName, Search searchReq, string paginationToken = "");
        Task<T> Add(VM entity);
        Task<IEnumerable<T>> Add(IEnumerable<VM> entities);
        Task Remove(Guid hashid, Guid sortid);
        Task<T> Update(Guid hashid, Guid sortid, VM entity);
    }
}