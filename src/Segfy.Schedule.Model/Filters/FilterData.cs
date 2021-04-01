using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Schedule.Model.Filters
{
    public class FilterData
    {
        public FilterData()
        {
            Filters = new List<Filter>();
            Sorts = new List<Sort>();
        }

        public int Offset { get; set; }

        public int Limit { get; set; }

        public IEnumerable<Sort> Sorts { get; set; }

        public IList<Filter> Filters { get; set; }
    }
}
