namespace Aban.Common.Utility
{
    public class StringUtility
    {
        public static string ReplacePersianNumberToEnglishNumber(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return text;
            }

            return text.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3")
                .Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("۷", "7")
                .Replace("۸", "8").Replace("۹", "9");
        }
        public static  string ReplaceEnglishNumberToPersianNumber(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return text;
            }

            return text.Replace("0", "۰").Replace("1", "۱").Replace("2", "۲").Replace("3", "۳")
                .Replace("4", "۴").Replace("5", "۵").Replace("6", "۶").Replace("7", "۷")
                .Replace("8", "۸").Replace("9", "۹");
        }

    }
}
