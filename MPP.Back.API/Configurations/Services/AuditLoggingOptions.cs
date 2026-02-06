namespace MPP.Back.API.Configurations.Services
{
    public sealed class AuditLoggingOptions
    {
        public const string SectionName = "AuditLogging";

        public bool Enabled { get; set; } = true;
        public bool LogRequestBody { get; set; } = true;
        public bool LogResponseBody { get; set; } = true;
        public int BodySizeLimit { get; set; } = 4096;
        public string[] ExcludedPaths { get; set; } = Array.Empty<string>();
    }
}
