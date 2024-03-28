namespace AccountSeller.Application.Common.Models
{
    public class SubjectItem
    {
        public string Code { get; set; } = string.Empty;

        public string? Name { get; set; }

        public byte TaxClassificationCode { get; set; }
    }
}
