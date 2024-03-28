using AccountSeller.Application.Common.Helpers;

namespace AccountSeller.Application.Common.Models
{
    public class EditableValueModel<T> : IEditableModel
    {
        public T? Value { get; set; }
        public bool IsEdited { get; set; } = false;

        // Add new parameterless constructor for model binding
        public EditableValueModel() { }

        public EditableValueModel(T? initialValue, bool initIsEdited = false)
        {
            Value = initialValue;
            IsEdited = initIsEdited;

            if (!EqualityComparer<T>.Default.Equals(initialValue, default(T)) && initialValue is string strValue)
            {
                Value = (T)(object)(strValue.Trim());
            }
        }

        public EditableValueModel(T? initialValue, object? updatedValue, bool isUpdateRecordExist)
        {
            if (isUpdateRecordExist)
            {
                // Both of them are null or have the same value.
                if (updatedValue == null || updatedValue?.GetHashCode() == initialValue?.GetHashCode())
                {
                    Value = initialValue;
                }
                // Have been changed.
                else
                {
                    Value = updatedValue != null ? (T?)updatedValue : default;
                    IsEdited = true;
                }
            }
            else
            {
                Value = initialValue;
            }

            if (Value is not null && Value is string)
            {
                Value = (T?)(object)Value.ToString().TrimEndNullAble();
            }
        }


        public void Edit(object? newValue)
        {
            if (newValue == null)
            {
                return;
            }

            if (this.Value is string)
            {
                this.Value = (T?)(object?)newValue?.ToString();
            }
            else
            {
                this.Value = (T)newValue;
            }    

            this.IsEdited = true;
        }
    }

    public interface IEditableModel
    {
        public bool IsEdited { get; set; }
    }
}
