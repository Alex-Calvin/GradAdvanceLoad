namespace DataProcessor.Models
{
    public class UserRecord
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ExternalId { get; set; } = string.Empty;
        public string ProcessedIndicator { get; set; } = string.Empty;
    }
} 