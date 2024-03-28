using System.Reflection;

namespace AccountSeller.Application.Common.Helpers
{
    public static class ReflectionHelper
    {
        public static IEnumerable<PropertyInfo> GetProperties<T>(BindingFlags bindingFlags, Func<PropertyInfo, bool> predicate) where T : class
        {
            return typeof(T).GetProperties(bindingFlags).Where(predicate);
        }

        public static IEnumerable<PropertyInfo> GetPublicProperties<T>(Func<PropertyInfo, bool> predicate) where T : class
        {
            return GetProperties<T>(BindingFlags.Instance | BindingFlags.Public, predicate);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesOfType<T>(Type type) where T : class
        {
            return GetPublicProperties<T>(e => e.PropertyType.IsAssignableTo(type));
        }

        /// <summary>
        /// Convert a <see cref="Enum"/> instance to <see cref="byte"/> instance.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static byte ConvertToByte<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            return Convert.ToByte(enumValue);
        }

        /// <summary>
        /// Convert a <see cref="Enum"/> instance to <see cref="Int32"/> instance.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static Int32 ConvertToInt32<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            return Convert.ToInt32(enumValue);
        }
        
        /// <summary>
        /// Try to get a value of inputted property name (<paramref name="propertyName"/>).
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value">If try get failed, this output parameter will be <see cref="default"/></param>
        /// <returns>
        /// <c>True</c>: Get property value succeed. <br></br>
        /// <c>False</c>: Get property value failed. <br></br>
        /// </returns>
        public static bool TryGetPropertyValue<TClass>(this TClass obj, string propertyName, out object? value) where TClass : class
        {
            PropertyInfo? prop = obj.GetType().GetProperty(propertyName);

            if (prop == null)
            {
                value = default;
                return false;
            }

            value = prop.GetValue(obj, null);
            return true;
        }
    }
}
