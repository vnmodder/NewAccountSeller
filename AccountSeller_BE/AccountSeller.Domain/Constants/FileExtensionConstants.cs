namespace AccountSeller.Domain.Constants
{
    public static class FileExtensionConstants
    {
        public const string Excel = ".xlsx";
        public const string CSV = ".csv";
        public const string Word = ".docx";
        public const string Text = ".txt";
        public const string PDF = ".pdf";
        public const string PNG = ".png";

        public static Dictionary<string, string> FileExtensions { get; } = new Dictionary<string, string>()
        {
            { ".xls", "application/vnd.ms-excel" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".csv", "text/csv; charset=utf-8" },
            { ".doc", "application/msword" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".json", "application/json" },
            { ".jsonld", "application/ld+json" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".jpg", "image/jpeg" },
            { ".pdf", "image/pdf" },
            { ".txt", "text/plain" },
        };
    }
}
