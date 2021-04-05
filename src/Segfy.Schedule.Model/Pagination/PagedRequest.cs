using System.Collections.Generic;

namespace Segfy.Schedule.Model.Pagination
{
    public class PagedRequest<T>
    {
        public IEnumerable<T> Items { get; set; }
    }
}