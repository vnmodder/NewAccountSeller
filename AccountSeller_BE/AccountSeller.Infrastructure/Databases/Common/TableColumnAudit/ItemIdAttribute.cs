using System.Reflection;

namespace AccountSeller.Infrastructure.Databases.Common.TableColumnAudit
{
    /// <summary>
    /// 項目ID | Item ID attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ItemIdAttribute : Attribute
    {
        /// <summary>
        /// 項目ID | Item ID.
        /// </summary>
        public string ItemId { get; }

        /// <summary>
        /// Property have this attribute.
        /// </summary>
        public PropertyInfo Property { get; set; } = null;

        public ItemIdAttribute(string itemId)
        {
            ItemId = itemId;
        }
    }
}
