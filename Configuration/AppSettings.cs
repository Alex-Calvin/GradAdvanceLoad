namespace DataProcessor.Configuration
{
    public class ApiSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
    }

    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
    }

    public class RetrySettings
    {
        public int MaxRetryDelayMs { get; set; } = 32000;
        public int JitterMaxMs { get; set; } = 1000;
        public int BaseDelayMs { get; set; } = 1000;
    }

    public class AppSettings
    {
        public ApiSettings ApiSettings { get; set; } = new();
        public DatabaseSettings DatabaseSettings { get; set; } = new();
        public RetrySettings RetrySettings { get; set; } = new();
    }
} 