using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace Aban.Common.HtmlHelper
{
    public static class ConvertDateTime
    {

        /// <summary>
        /// شنبه 12 شهریور 1401 - 17:43
        /// </summary>
        /// <returns></returns>
        public static IHtmlContent ConvertPersianDateTime(
            this IHtmlHelper html,
            DateTime? dateTime)
        {
            if (dateTime != null)
            {
                IDateTime iDateTime = new IDateTime(dateTime.Value);
                return new HtmlString(iDateTime.SpecialFormat1 + " - " + string.Format("{0:H:mm}", dateTime.Value));
            }
            else
            {
                return new HtmlString("تاریخ ثبت نشده است");
            }
        }


        /// <summary>
        /// شنبه 23 شهریور 1398
        /// </summary>
        /// <param name="html"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static IHtmlContent ConvertPersianDate(this IHtmlHelper html, DateTime dateTime)
        {

            IDateTime iDateTime = new IDateTime(dateTime);
            return new HtmlString(iDateTime.SpecialFormat1);
        }


        /// <summary>
        /// 1399/01/01
        /// </summary>
        /// <returns></returns>
        public static IHtmlContent ConvertToPersianDate(this IHtmlHelper html, DateTime dateTime, string format = "0")
        {
            PersianCalendar pc = new PersianCalendar();
            string year = pc.GetYear(dateTime).ToString("0000");
            string month = pc.GetMonth(dateTime).ToString(format);
            string day = pc.GetDayOfMonth(dateTime).ToString(format);
            string result = $"{year}/{month}/{day}";
            return new HtmlString(result);
        }


        /// <summary>
        /// 1399/01/01 - 00:00
        /// </summary>
        /// <returns></returns>
        public static IHtmlContent ConvertToPersianDateTime(this IHtmlHelper html, DateTime dateTime, string format = "0")
        {
            PersianCalendar pc = new PersianCalendar();
            string year = pc.GetYear(dateTime).ToString("0000");
            string month = pc.GetMonth(dateTime).ToString(format);
            string day = pc.GetDayOfMonth(dateTime).ToString(format);
            string result = $"{year}/{month}/{day} - {string.Format("{0:H:mm}", dateTime)}";
            return new HtmlString(result);
        }
    }
}
