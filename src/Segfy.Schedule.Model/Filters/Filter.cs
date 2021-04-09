using Segfy.Schedule.Model.Enuns;

namespace Segfy.Schedule.Model.Filters
{
    public class Filter
    {
        public string Field { get; set; }

        public string Value { get; set; }

        public OperatorType Operator { get; set; } = OperatorType.Equal;
    }
}