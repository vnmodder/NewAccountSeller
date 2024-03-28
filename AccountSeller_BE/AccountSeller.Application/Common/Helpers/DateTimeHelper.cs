using System.Globalization;

namespace AccountSeller.Application.Common.Helpers
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Get current server time with format "yyyy-MM-dd". This is the Japanese time too.
        /// </summary>
        public static DateTime Now => DateTime.UtcNow.AddHours(9);

        /// <summary>
        /// Generate first date of <paramref name="monthAdd"/> months after <paramref name="date"/>
        /// </summary>
        /// <param name="date"></param>
        /// <param name="monthAdd"></param>
        /// <returns></returns>
        public static DateTime AddMonth(DateTime date, int monthAdd)
            => DateTime.Parse(date.AddMonths(monthAdd).ToString("yyyy-MM-01"));

        public static DateTime AddMonthOnly(DateTime data, int monthAdd)
            => DateTime.Parse(data.AddMonths(monthAdd).ToString("yyyy-MM-dd"));

        public static string ToFileNameTime(this DateTime value)
            => value.ToString("yyyyMMddHHmmss");

        public static string ToHyphenDateFormat(this DateTime value)
            => value.ToString("yyyy-MM-dd");

        public static string ToHyphenDateFormat(this DateTime? value)
            => value.HasValue ? value.Value.ToString("yyyy-MM-dd") : string.Empty;

        /// <summary>
        /// Convert a <see cref="DateTime"/> instance to <see cref="string"/> instance with format: [yyyy/MM/dd].
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSlashDateFormat(this DateTime value)
            => value.ToString("yyyy/MM/dd");

        /// <summary>
        /// Convert a <see cref="DateTime"/> instance to <see cref="string"/> instance with format: [yyyy/MM/dd].
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSlashDateFormat(this DateTime? value)
            => value.HasValue ? value.Value.ToSlashDateFormat() : string.Empty;

        /// <summary>
        /// Convert <see cref="DateTime"/> to <see cref="String"/> with format: [yyyy年MM月dd日]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJapaneseDateFormat(this DateTime value)
            => value.ToString("yyyy年MM月dd日");

        /// <summary>
        /// Convert <see cref="DateTime"/> to <see cref="String"/> with format: [yyyy年MM月dd日]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJapaneseDateFormat(this DateTime? value)
            => value.HasValue ? value.Value.ToJapaneseDateFormat() : string.Empty;

        /// <summary>
        /// Convert <see cref="DateTime"/> to <see cref="String"/> with format: [yyyy年M月d日]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJapaneseSingleDateFormat(this DateTime value)
            => value.ToString("yyyy年M月d日");

        /// <summary>
        /// Convert <see cref="DateTime"/> to <see cref="String"/> with format: [yyyy年M月d日]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJapaneseSingleDateFormat(this DateTime? value)
            => value.HasValue ? value.Value.ToJapaneseSingleDateFormat() : string.Empty;

        /// <summary>
        /// Convert <see cref="DateTime"/> to <see cref="String"/> with format: [yyyy年MM月]
        /// </summary>
        /// <param name="value"></param>
        public static string ToJapaneseMonthFormat(this DateTime value)
            => value.ToString("yyyy年MM月");

        /// <summary>
        /// Convert <see cref="DateTime?"/> to <see cref="String"/> with format: [yyyy年MM月]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJapaneseMonthFormat(this DateTime? value)
            => value.HasValue ? value.Value.ToJapaneseMonthFormat() : string.Empty;

        /// <summary>
        /// Convert <see cref="DateTime"/> to <see cref="String"/> with format: [yyyy年MM月]
        /// </summary>
        /// <param name="value"></param>
        public static string ToJapaneseSingleMonthFormat(this DateTime value)
            => value.ToString("yyyy年M月");

        /// <summary>
        /// Convert <see cref="DateTime?"/> to <see cref="String"/> with format: [yyyy年MM月]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJapaneseSingleMonthFormat(this DateTime? value)
            => value.HasValue ? value.Value.ToJapaneseDateFormat() : string.Empty;

        /// <summary>
        /// Convert a <see cref="DateTime"/> instance to Japanese month format for 摘要 (Billing summary) value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Date in string format: yyyy年MM月分</returns>
        public static string ToJapaneseMonthSummaryFormat(this DateTime value)
            => value.ToString("yyyy年MM月分");

        /// <summary>
        /// Convert a <see cref="DateTime"/> instance to Japanese month format for 摘要 (Billing summary) value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Date in string format: yyyy年MM月分</returns>
        public static string ToJapaneseMonthSummaryFormat(this DateTime? value)
            => value.HasValue ? value.Value.ToString("yyyy年MM月分") : string.Empty;

        /// <summary>
        /// Compare year and month of 2 <see cref="DateTime"/> values.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueToCompare"></param>
        /// <returns>
        /// <list type="bullet">
        /// <item>0: 2 value are equal</item>
        /// <item>1: This <paramref name="value"/> is greater than <paramref name="valueToCompare"/></item>
        /// <item>-1: This <paramref name="value"/> is less than <paramref name="valueToCompare"/></item>
        /// </list>
        /// </returns>
        public static int MonthYearCompare(this DateTime value, DateTime valueToCompare)
        {
            value = DateTime.Parse(value.ToString("yyyy-MM-dd"));
            valueToCompare = DateTime.Parse(valueToCompare.ToString("yyyy-MM-dd"));
            if (value == valueToCompare)
            {
                return 0;
            }
            else if (value > valueToCompare)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Compare year and month of 2 <see cref="DateTime"/> values.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueToCompare"></param>
        /// <returns>
        /// <list type="bullet">
        /// <item>0: 2 value are equal</item>
        /// <item>1: This <paramref name="value"/> is greater than <paramref name="valueToCompare"/></item>
        /// <item>-1: This <paramref name="value"/> is less than <paramref name="valueToCompare"/></item>
        /// </list>
        /// </returns>
        public static int MonthYearCompare(this DateTime? value, DateTime? valueToCompare)
        {
            value ??= DateTime.MinValue;
            valueToCompare ??= DateTime.MinValue;
            return value.Value.MonthYearCompare(valueToCompare.Value);
        }

        /// <summary>
        /// Compare year and month of 2 <see cref="DateTime"/> values.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueToCompare"></param>
        /// <returns>
        /// <list type="bullet">
        /// <item>0: 2 value are equal</item>
        /// <item>1: 2 <paramref name="value"/> is greater than <paramref name="valueToCompare"/></item>
        /// <item>1: 2 <paramref name="value"/> is less than <paramref name="valueToCompare"/></item>
        /// </list>
        /// </returns>
        public static int MonthYearCompare(this DateTime value, DateTime? valueToCompare)
        {
            if (valueToCompare is null)
            {
                return 1;
            }
            else if (value.Month == valueToCompare.Value.Month && value.Year == valueToCompare.Value.Year)
            {
                return 0;
            }
            else if (value > valueToCompare)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public static int TotalSecondsDifference(this DateTime dateTime1, DateTime dateTime2)
        {
            TimeSpan timeDifference = dateTime2 - dateTime1;
            return (int)timeDifference.TotalSeconds;
        }

        public static bool BeValidDateTime(DateTime dateTime)
        {
            const string dateTimeFormat = "yyyy-MM-dd";
            if (DateTime.TryParseExact(dateTime.ToString(dateTimeFormat), dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
            {
                return parsedDateTime == dateTime;
            }

            return false;
        }

        /// <summary>
        /// Convert a time value to Japanese era date string.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="format">Format to output string.</param>
        /// <returns></returns>
        public static string ToJapaneseEraDateString(this DateTime time, string format = "ggyy年M月d日")
        {
            var culture = new CultureInfo("ja-JP");
            culture.DateTimeFormat.Calendar = new JapaneseCalendar();
            return time.ToString(format, culture);
        }

        /// <summary>
        /// Get Japan era index from a <see cref="DateTime"/> instance.
        /// </summary>
        /// <param name="timeValue"></param>
        /// <returns></returns>
        public static int GetJapanEraIndex(this DateTime timeValue)
        {
            var culture = new CultureInfo("ja-JP");
            culture.DateTimeFormat.Calendar = new JapaneseCalendar();

            return culture.DateTimeFormat.Calendar.GetEra(timeValue);
        }
        
        /// <summary>
        /// Get Japan era year value from a <see cref="DateTime"/> instance.
        /// </summary>
        /// <param name="timeValue"></param>
        /// <returns></returns>
        public static int GetJapanEraYear(this DateTime timeValue)
        {
            var culture = new CultureInfo("ja-JP");
            culture.DateTimeFormat.Calendar = new JapaneseCalendar();

            return culture.DateTimeFormat.Calendar.GetYear(timeValue);
        }

        /// <summary>
        /// Convert a time value to Japanese era date string.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="format">Format to output string.</param>
        /// <returns></returns>
        public static string ToJapaneseEraDateString(this DateTime? time, string format = "ggyy年M月d日")
        {
            if (!time.HasValue)
            {
                return string.Empty;
            }

            var culture = new CultureInfo("ja-JP");
            culture.DateTimeFormat.Calendar = new JapaneseCalendar();

            return time.Value.ToString(format, culture);
        }

        /// <summary>
        /// Get end date of month of selected date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime EndDateOfMonth(this DateTime date)
        {
            var newDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            return DateTime.Parse(newDate.ToString("yyyy-MM-dd"));
        }

        /// <summary>
        /// Get end date of month of selected date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime EndDateOfMonth(this DateTime? date)
        {
            if (!date.HasValue)
            {
                return DateTime.MinValue;
            }
            var newDate = new DateTime(date.Value.Year, date.Value.Month, DateTime.DaysInMonth(date.Value.Year, date.Value.Month));
            return DateTime.Parse(newDate.ToString("yyyy-MM-dd"));
        }

        /// <summary>
        /// Set date of <see cref="DateTime"/> value to 01.
        /// </summary>
        /// <param name="dateInstance"></param>
        /// <returns></returns>
        public static DateTime ToFirstDateOfMonth(this DateTime dateInstance)
            => new(dateInstance.Year, dateInstance.Month, 1);

        /// <summary>
        /// Set date of <see cref="DateTime"/> value to 01.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? ToFirstDateOfMonth(this DateTime? value)
            => value.HasValue ? value.Value.ToFirstDateOfMonth() : null;

        /// <summary>
        /// Set date of <see cref="DateTime"/> value to the last day of the month.
        /// </summary>
        /// <param name="dateInstance"></param>
        /// <returns></returns>
        public static DateTime ToLastDateOfMonth(this DateTime dateInstance)
        {
            return new DateTime(dateInstance.Year, dateInstance.Month, 1).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Set date of <see cref="DateTime"/> value to the last day of the month.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? ToLastDateOfMonth(this DateTime? value)
        {
            return value.HasValue ? value.Value.ToLastDateOfMonth() : (DateTime?)null;
        }

        /// <summary>
        /// Remove miliseconds from datetime
        /// </summary>
        /// <param name="value">datetime value to be removed miliseconds</param>
        /// <returns>Value after remove miliseconds</returns>
        public static DateTime? TruncateMiliseconds(this DateTime? value)
        {
            if (value == null)
            {
                return null;
            }
            return value.Value.AddTicks(-(value.Value.Ticks % TimeSpan.TicksPerSecond));
        }

        /// <summary>
        /// Get the number of month between <paramref name="startDate"/> to <paramref name="endDate"/>.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int GetMonthDifferenceFrom(this DateTime startDate, DateTime endDate)
        {
            int yearsDifference = endDate.Year - startDate.Year;
            int monthsDifference = endDate.Month - startDate.Month;

            if (monthsDifference < 0)
            {
                yearsDifference--;
                monthsDifference += 12;
            }

            return (yearsDifference * 12) + monthsDifference;
        }

        /// <summary>
        /// Set the time partition of <paramref name="value"/> to mid night
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToStartOfDate(this DateTime value)
        {
            return value.AddTicks(-(value.Ticks % TimeSpan.TicksPerDay));
        }

        /// <summary>
        /// Set the time partition of <paramref name="value"/> to mid night
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? ToStartOfDate(this DateTime? value)
        {
            return value?.ToStartOfDate();
        }

        public static DateTime FirstDateOfYear(int year)
        {
            return new DateTime(year, 1, 1);
        }

        public static DateTime FirstDateOfMonth(int year, int month)
        {
            return new DateTime(year, month, 1);
        }

        public static int? GetMonthNullAble(this DateTime? date)
        {
            return date == null ? null : date.Value.Month;
        }

        public static int? GetYearNullAble(this DateTime? date)
        {
            return date == null ? null : date.Value.Year;
        }
    }
}
