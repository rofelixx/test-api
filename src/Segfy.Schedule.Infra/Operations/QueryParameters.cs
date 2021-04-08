using System;

namespace Segfy.Schedule.Infra.Operations
{
    public class QueryParameters
    {
        public Guid HashKey { get; set; }
        public int PerPage { get; set; }
        public Guid LastRangeKey { get; set; }
    }
}