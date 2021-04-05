namespace Segfy.Schedule.Model.Pagination
{
    public class DynamoDBPagedRequest<T> : PagedRequest<T>
    {
        public string PaginationToken { get; set; }
        public int TotalSegments { get; set; }
        public int Segment { get; set; }
        public bool IsDone { get; set; }
    }
}