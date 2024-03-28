

using AccountSeller.Application.Common.Models;

namespace AccountSeller.Application.Common.Helpers
{
    public static class EditableValueModelHelper
    {
        /// <summary>
        /// Create instance of <see cref="EditableValueModel{T}"/>, isEdited field will be set to true if <paramref name="normalTableData"/> have corresponding data
        /// </summary>
        /// <typeparam name="TSource">Type of object contains value in result</typeparam>
        /// <typeparam name="TValue">Type of value in result </typeparam>
        /// <param name="normalTableData"></param>
        /// <param name="updateTableData"></param>
        /// <param name="valueSelector"></param>
        /// <returns></returns>
        public static EditableValueModel<TValue> CreateEditableValueModel<TSource, TValue>(
            TSource? normalTableData,
            TSource? updateTableData,
            Func<TSource?, TValue> valueSelector)
        {
            var normalTableValue = valueSelector(normalTableData);
            var updateTableValue = valueSelector(updateTableData);

            return new EditableValueModel<TValue>(updateTableValue ?? normalTableValue, updateTableValue != null);
        }
    }
}
