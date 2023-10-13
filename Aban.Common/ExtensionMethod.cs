using Aban.Common.Utility;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Aban.Common
{
    public static class ExtensionMethod
    {

        public static string ReplacePersianNumberToEnglishNumber(this string text)
            => StringUtility.ReplacePersianNumberToEnglishNumber(text);


        public static string ReplaceEnglishNumberToPersianNumber(this string text)
            => StringUtility.ReplaceEnglishNumberToPersianNumber(text);

        public static string GetDescription<T>(this T enumValue)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                return null;

            var description = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo != null)
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    description = ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return description;
        }

        public static bool IsDigit(string text)
            => text.ToArray().All(char.IsDigit);

        public static bool IsNullOrEmpty(this string? value)
            => (value == null || 0 == value.Length) ? true : false;
    }
}
