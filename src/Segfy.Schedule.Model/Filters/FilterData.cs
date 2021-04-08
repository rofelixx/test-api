using System;
using System.Collections.Generic;

namespace Segfy.Schedule.Model.Filters
{
    public class FilterData
    {
        public FilterData()
        {
            Filters = new List<Filter>();
            Sorts = new List<Sort>();
        }

        public Guid? LastKey { get; set; }

        public int? Limit { get; set; }

        public IEnumerable<Sort> Sorts { get; set; }

        public IList<Filter> Filters { get; set; }
    }
}
