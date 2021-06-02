using System;
using System.Collections.Generic;

namespace Segfy.Schedule.Model.Dtos
{
    public class PaginationDto<T>
    {
        public IEnumerable<T> Items { get; set; }
        public Guid? NextKey { get; set; }
    }
}