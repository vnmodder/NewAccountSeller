using System.Text;

namespace AccountSeller.Application.Common.Helpers
{
    public static class StringUtils
    {
        public const char NEW_LINE_CHARACTER = '\n';

        public static string TrimNullAble(this string? value)
        {
            return (value == null) ? null : value.Trim();
        }

        public static string TrimEndNullAble(this string? value)
        {
            return (value == null) ? null : value.TrimEnd();
        }

        /// <summary>
        /// Replace <paramref name="valueToBeReplaced"/> to <paramref name="replaceValue"/> for all properties that are type <see cref="String"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="valueToBeReplaced"></param>
        /// <param name="replaceValue"></param>
        public static void ReplaceValueForStringProps<T>(
            this T instance,
            string valueToBeReplaced,
            string replaceValue) where T : class
        {
            foreach (var prop in instance.GetType().GetProperties())
            {
                if (prop.GetType() != typeof(String))
                {
                    continue;
                }

                var tempValue = prop.GetValue(instance) as string;
                tempValue = tempValue.Replace(valueToBeReplaced, replaceValue);

                prop.SetValue(instance, tempValue);
            }
        }

        /// <summary>
        /// Convert ChaBiko1, ChaBiko2, ... to one string with each value separated by a new line character
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">object contains ChaBiko1, ChaBiko2, ...</param>
        /// <returns>A string contains all Biko data</returns>
        public static string JoinTextColumns<T>(this T source, string textColumnNamePrefix) where T : class
        {
            var noteColumns = ReflectionHelper.GetPublicProperties<T>(e => e.PropertyType == typeof(string) && e.Name.StartsWith(textColumnNamePrefix));
            var stringBuilder = new StringBuilder();

            foreach (var column in noteColumns)
            {
                var value = (string?)column.GetValue(source);

                var trimmedValue = value?.TrimEnd();

                stringBuilder.AppendLine(trimmedValue);
            }

            return stringBuilder.ToString().TrimEnd();
        }

        /// <summary>
        /// Assign each line of text into each text column
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="destination">Object contains text columns which you want to assign to</param>
        /// <param name="text">String contain multiple line</param>
        /// <param name="textColumnNamePrefix">prefix to mark which property is text column, example: ChaBiko, ...</param>
        public static void AssignTextColumns<T>(this T destination, string? text, string textColumnNamePrefix) where T : class
        {
            var textColumns = ReflectionHelper.GetPublicProperties<T>(e => e.PropertyType == typeof(string) && e.Name.StartsWith(textColumnNamePrefix));

            var textLines = text?.Split(NEW_LINE_CHARACTER);

            var textLinesIndex = 0;

            foreach (var noteColumn in textColumns)
            {
                if (string.IsNullOrEmpty(text) || textLinesIndex >= textLines!.Length)
                {
                    noteColumn.SetValue(destination, null);
                    continue;
                }

                noteColumn.SetValue(destination, textLines![textLinesIndex++]);
            }
        }

        /// <summary>
        /// Truncate a <see cref="string"/> instance if it's byte length is over <paramref name="maxLength"/>. 
        /// <br></br>
        /// Return a new instance after truncating.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength"></param>
        /// <returns><see cref="string"/> instance after truncating.</returns>
        public static string TruncateOverString(this string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            var bytes = Encoding.GetEncoding("Shift_JIS").GetBytes(str);

            if (maxLength > bytes.Length)
            {
                maxLength = bytes.Length;
            }
            var tempResult = Encoding.GetEncoding("Shift_JIS").GetString(bytes, 0, maxLength);

            if (tempResult.GetStringByteLength() > 60)
            {
                tempResult = tempResult[..^1];
            }

            return tempResult;
        }

        /// <summary>
        /// Convert a <see cref="long"/> instance to <see cref="string"/> instance with format [###,###,###,###].
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToMoneyNumberFormat(this long number) => number.ToString("###,###,###,###");

        /// <summary>
        /// Convert a <see cref="long"/> instance to <see cref="string"/> instance with format [###,###,###,###].
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToMoneyNumberFormat(this long? number)
            => number.HasValue ? number.Value.ToString("###,###,###,###") : string.Empty;

        /// <summary>
        /// Convert a <see cref="long"/> instance to <see cref="string"/> instance with format [###,###,##0円].
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJapaneseMoneyFormat(this long value) => value.ToString("###,###,##0円");

        /// <summary>
        /// Convert a <see cref="decimal"/> instance to <see cref="string"/> instance with format [###,###,##0.00円].
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJapaneseMoneyFormat(this decimal value) => value.ToString("###,###,##0.00円");

        /// <summary>
        /// Convert a <see cref="decimal?"/> instance to <see cref="string"/> instance for area with format [####0.00㎡].
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToAreaSquareMeter(this decimal value) => value.ToString("####0.00㎡");

        /// <summary>
        /// Convert a <see cref="decimal?"/> instance to <see cref="string"/> instance for area with format [####0.00坪].
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToAreaSquareTsubo(this decimal value) => value.ToString("####0.00坪"); 
    }
}
