using System.Reflection;

namespace AccountSeller.Infrastructure.Databases.Common.TableColumnAudit
{
    public static class ItemIdHelper
    {
        public const string DB_DEFAULT_ITEM_ID = "0000";

        /// <summary>
        /// Get all <see cref="ItemIdAttribute"/> in a type (include <see cref="PropertyInfo"/>).
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<ItemIdAttribute> GetItemIdAttributes(this Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
               .Select(property =>
               {
                   var addressAttribute =
                       property.GetCustomAttributes(typeof(ItemIdAttribute), true).FirstOrDefault() as ItemIdAttribute;
                   if (addressAttribute != null)
                   {
                        addressAttribute!.Property = property;
                        return addressAttribute;
                   }
                   else
                   {
                       return null;
                   }
               }).Where(x => x != null).ToList();
        }

        /// <summary>
        /// Get a dictionary of a type include property name and item id. <br></br>
        /// Item id used for insert or update value in column <see cref="ShoninDataTable.ChaKomokuId"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> where: <br></br> 
        /// <seealso cref="Dictionary{TKey, TValue}.Keys"/> is property's name. <br></br> 
        /// <seealso cref="Dictionary{TKey, TValue}.ValueCollection"/> is item id. <br></br> 
        /// </returns>
        public static Dictionary<string, string> GetPropertyNameWithItemId(this Type type)
        {
            return type.GetItemIdAttributes().ToDictionary(key => key.Property.Name, value => value.ItemId);
        }
    }
}
