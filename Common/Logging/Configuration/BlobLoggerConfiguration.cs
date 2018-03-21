namespace Common.Logging.Configuration
{
    public class BlobLoggerConfiguration
    {
        public string ConnectionString { get; set; }

        public string Container { get; set; }

        public string FileName { get; set; }
    }
}
