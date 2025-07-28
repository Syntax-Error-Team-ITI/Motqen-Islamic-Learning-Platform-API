namespace MotqenIslamicLearningPlatform_API.DTOs.BotDTOs
{
    public class QueryContext
    {
        public string Question { get; set; }
        public string role { get; set; }
        public string Id { get; set; }
        public object DatabaseResult { get; set; }
        public string FormattedData { get; set; }
    }
}
