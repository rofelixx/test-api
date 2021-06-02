using Segfy.Schedule.Model.Filters;
using System;
using System.Collections.Generic;

namespace Segfy.Schedule.Infra.Operations
{
    public class QueryParameters
    {
        public Guid HashKey { get; set; }
        public int PerPage { get; set; }
        public Guid LastRangeKey { get; set; }

        public IList<Filter> Filters { get; set; }
    }
}