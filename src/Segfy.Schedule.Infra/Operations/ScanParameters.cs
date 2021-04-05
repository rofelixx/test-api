using System.Collections.Generic;

namespace Segfy.Schedule.Infra.Operations
{
    public class ScanParameters
    {
        public string PaginationToken { get; set; }
        public int PerPage { get; set; }
        public string IndexName { get; set; }
        public IEnumerable<Model.Filters.Filter> Filters { get; set; }
    }
}