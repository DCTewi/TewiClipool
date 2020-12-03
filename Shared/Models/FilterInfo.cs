namespace TewiClipool.Shared.Models
{
    public class FilterInfo
    {
        public FilterType Type { get; set; } = FilterType.ByTitle;

        public string FilterString { get; set; }

        public int Page { get; set; } = 0;
    }
}
