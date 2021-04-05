using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Segfy.Schedule.Model.Entities;
using Segfy.Schedule.Model.Pagination;

namespace Segfy.Schedule.Infra.Operations
{
    public interface IDynamoBDOperations<T> where T : BaseEntity
    {
        Task SaveAsync(T entity);
        Task SaveAsync(IEnumerable<T> entity);
        Task<DynamoDBPagedRequest<T>> ScanAsync(ScanParameters parameters);
        Task DeleteAsync(Guid hashid, Guid sortid);
        Task<T> LoadAsync(Guid hashid, Guid sortid);
    }
}