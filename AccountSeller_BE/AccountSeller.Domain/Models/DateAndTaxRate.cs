namespace AccountSeller.Domain.Models
{
    public class DateAndTaxRate
    {
        public DateTime? StartDate { get; set; }

        public decimal TaxRate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
