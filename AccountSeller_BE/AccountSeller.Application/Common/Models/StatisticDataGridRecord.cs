namespace AccountSeller.Application.Common.Models
{
    public class StatisticDataGridRecord
    {
        public string Key { get; set; } = String.Empty;
        public long OriginalBudget { get; set; }
        public long AdditionalBudget { get; set; }

        public long TotalBudget { get; set; }
    }
}
