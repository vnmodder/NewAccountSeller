using Newtonsoft.Json;
using System.Reflection;

namespace AccountSeller.Application.Common.Helpers
{
    public static class JsonHelper
    {
        public static string ConvertToString<T>(this T obj, bool ignoreNull = false)
        {
            if (Equals(obj, default(T)))
            {
                return string.Empty;
            }

            if (ignoreNull)
            {
                return JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }

            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Trim all property value which have property data type is <see cref="String"/>.
        /// </summary>
        /// <param name="obj"></param>
        public static void TrimAllStringsOfAnObject(object? obj)
        {
            if (obj == null)
            {
                return;
            }

            PropertyInfo[] properties = obj.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    string? value = (string?)property.GetValue(obj);
                    if (value != null)
                    {
                        string trimmedValue = value.TrimEndNullAble();
                        property.SetValue(obj, trimmedValue);
                    }
                }
            }
        }

        public static List<string> GetNullablePropertyNames<T>(T obj)
        {
            List<string> nullablePropertyNames = new();

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                if (Nullable.GetUnderlyingType(property.PropertyType) != null || property.PropertyType == typeof(string))
                {
                    nullablePropertyNames.Add(property.Name);
                }
            }

            return nullablePropertyNames;
        }

        /// <summary>
        /// Convert a <see cref="bool"/> instance to a <see cref="byte"/> instance.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ConvertToByte(this bool? value)
        {
            return (byte)(value == true ? 1 : 0);
        }
        
        /// <summary>
        /// Convert a <see cref="bool"/> instance to a <see cref="byte"/> instance.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ConvertToByte(this bool value)
        {
            return (byte)(value ? 1 : 0);
        }

        /// <summary>
        /// Convert a <see cref="byte"/> instance to a <see cref="bool"/> instance for value used to be a flag.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ConvertToBoolean(this byte? value)
        {
            return value > 0;
        }
    }
}
