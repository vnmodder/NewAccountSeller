using AccountSeller.Application.Common.Models;
using AccountSeller.Domain.Exceptions;
using AccountSeller.Domain.Messages;
using System.Text;

namespace AccountSeller.Application.Common.Helpers
{
    public static class ValidationHelper
    {
        private const string JAPANESE_NOTE_COLUMN_NAME = "備考";
        private static readonly Encoding JapaneseTextEncoding;

        static ValidationHelper()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            JapaneseTextEncoding = Encoding.GetEncoding("UTF-8");
        }

        public static int GetStringByteLength(this string? str)
        {
            return !string.IsNullOrEmpty(str) ? JapaneseTextEncoding.GetByteCount(str) : 0;
        }

        public static void ValidateNote<T>(this string? note, string noteColumnNamePrefix, int maxBytesPerLine) where T : class
        {
            if (string.IsNullOrWhiteSpace(note))
            {
                return;
            }

            var maxAllowedLines = ReflectionHelper.GetPublicProperties<T>(e => e.PropertyType == typeof(string) && e.Name.StartsWith(noteColumnNamePrefix)).Count();

            var noteLines = note.Split(StringUtils.NEW_LINE_CHARACTER);

            if (noteLines.Length > maxAllowedLines)
            {
                throw new BusinessException(string.Format(ValidationMessages.VM0008, JAPANESE_NOTE_COLUMN_NAME));
            }

            for (int i = 0; i < noteLines.Length; i++)
            {
                var line = noteLines[i];

                if (line.GetStringByteLength() > maxBytesPerLine)
                {
                    throw new BusinessException(string.Format(ValidationMessages.VM0009, JAPANESE_NOTE_COLUMN_NAME, i + 1));
                }
            }
        }

        /// <summary>
        /// Check that the object have any edited field by manipulating <see cref="IEditable" /> properties
        /// </summary>
        /// <typeparam name="TObjec">Type of object contains IEditable properties</typeparam>
        /// <param name="obj">Object contains IEditable properties to be checked</param>
        /// <returns>If have any isEdited return true, otherwise false</returns>
        public static bool HaveAnyEditedField<TObject>(this TObject obj) where TObject : class
        {
            var editableProperties = ReflectionHelper.GetPropertiesOfType<TObject>(typeof(IEditableModel));

            return editableProperties.Any(e => (e.GetValue(obj) as IEditableModel)!.IsEdited);
        }
    }
}
