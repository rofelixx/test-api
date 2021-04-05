using System;

namespace Segfy.Schedule.Infra.Operations
{
    public class QueryParameters
    {
        public Guid HashKey { get; set; }
        public string PaginationToken { get; set; }
        public int PerPage { get; set; }
    }
}