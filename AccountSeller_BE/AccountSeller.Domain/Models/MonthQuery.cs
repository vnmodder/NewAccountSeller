using Microsoft.AspNetCore.Mvc;

namespace AccountSeller.Domain.Models
{
    public class MonthQuery
    {
        [FromQuery]
        public int Month { get; set; }

        [FromQuery]
        public int Year { get; set; }

        public DateTime ToDateTime(bool firstDay)
        {
            if (firstDay)
            {
                return new DateTime(Year, Month, 1);
            }
            else
            {
                var lastDayOfMonth = DateTime.DaysInMonth(Year, Month);
                return new DateTime(Year, Month, lastDayOfMonth);
            }
        }
    }
}
