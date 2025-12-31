using Aban.Domain.Entities;
using Aban.Domain.Enumerations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Common.Utility
{
    public static class DateTimeUtility
    {

        public static string ToPerianTime(this string time)
        {
            return time.Replace("AM", "قبل از ظهر").Replace("PM", "بعد از ظهر");
        }

        public static TimeSpan ToConvertStringToTime(this string time)
        {
            if (String.IsNullOrEmpty(time))
            {
                return new TimeSpan(0, 0, 0);
            }
            return TimeSpan.Parse(time);
        }

        public static DateTime MergeDateAndTime(this DateTime date, TimeSpan time)
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>Item1: year,Item2: month,Item3: day</returns>
        public static Tuple<int, int, int> ToDateTimeToPersianCalendar(this DateTime dateTime)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = MessageTypeResult.Success;
            try
            {
                PersianCalendar pc = new PersianCalendar();
                int year = pc.GetYear(dateTime);
                int month = pc.GetMonth(dateTime);
                int day = pc.GetDayOfMonth(dateTime);
                return Tuple.Create(year, month, day);
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gregorianYear"></param>
        /// <param name="gregorianMonth"></param>
        /// <param name="gregorianDay"></param>
        /// <returns>Item1: year,Item2: month,Item3: day</returns>
        public static Tuple<int, int, int> ToDateTimeToPersianCalendar(this int gregorianYear, int gregorianMonth = 1, int gregorianDay = 1)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = MessageTypeResult.Success;
            try
            {
                DateTime dateTime = new DateTime(gregorianYear, gregorianMonth, gregorianDay);
                PersianCalendar pc = new PersianCalendar();
                int year = pc.GetYear(dateTime);
                int month = pc.GetMonth(dateTime);
                int day = pc.GetDayOfMonth(dateTime);
                return Tuple.Create(year, month, day);
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public static string ToConvertDateTimeToPersianDate(
            this DateTime dateTime,
            DateTimeFormat outputDateFormat = DateTimeFormat.yyyy_mm_dd,
            DateTimeSpiliter inputDateTimeSpiliter = DateTimeSpiliter.slash)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = MessageTypeResult.Success;
            try
            {
                PersianCalendar pc = new PersianCalendar();
                string year = pc.GetYear(dateTime).ToString();
                string month = pc.GetMonth(dateTime).ToString();
                string day = pc.GetDayOfMonth(dateTime).ToString();

                #region Spilitter

                char spiliter = inputDateTimeSpiliter switch
                {
                    DateTimeSpiliter.slash => '/',
                    DateTimeSpiliter.back_slash => '\\',
                    DateTimeSpiliter.colon => ':',
                    DateTimeSpiliter.dash => '-',
                    DateTimeSpiliter.dot => '.',
                    DateTimeSpiliter.semi => ',',
                    DateTimeSpiliter.semicolon => ';',
                    DateTimeSpiliter.star => '*',
                    DateTimeSpiliter.underline => '_',
                    _ => '/'
                };
                //return $"{day}{spiliter}{month}{spiliter}{year}";
                switch (outputDateFormat)
                {
                    case DateTimeFormat.dd_mm_yyyy:
                        return $"{day}{spiliter}{month}{spiliter}{year}";
                    case DateTimeFormat.mm_dd_yyyy:
                        return $"{month}{spiliter}{day}{spiliter}{year}";
                    case DateTimeFormat.yyyy_dd_mm:
                        return $"{year}{spiliter}{day}{spiliter}{month}";
                    case DateTimeFormat.yyyy_mm_dd:
                        return $"{year}{spiliter}{month}{spiliter}{day}";
                    case DateTimeFormat.dd_yyyy_mm:
                        return $"{day}{spiliter}{year}{spiliter}{month}";
                    case DateTimeFormat.mm_yyyy_dd:
                        return $"{month}{spiliter}{year}{spiliter}{day}";
                    default:
                        return $"{year}{spiliter}{month}{spiliter}{day}";
                }

                #endregion

#pragma warning disable CS0162 // Unreachable code detected
                return $"{year}{spiliter}{month}{spiliter}{day}";
#pragma warning restore CS0162 // Unreachable code detected
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public static string ToConvertDateTimeToPersianDateTime(
           this DateTime dateTime,
           DateTimeFormat outputDateFormat = DateTimeFormat.yyyy_mm_dd,
           DateTimeSpiliter inputDateTimeSpiliter = DateTimeSpiliter.slash)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = MessageTypeResult.Success;
            try
            {
                string date = dateTime.ToConvertDateTimeToPersianDate(outputDateFormat, inputDateTimeSpiliter);
                string dateAndTime = date + $" {String.Format("{0: - HH:mm:ss}", dateTime)}";
                return dateAndTime;
            }
            catch
            {
                return "خطا در پردازش تاریخ";
            }


        }




        public static IHtmlContent ToConvertDateTimeToPersianDate(
            this IHtmlHelper helper,
            DateTime dateTime,
            DateTimeFormat outputDateFormat,
            DateTimeSpiliter inputDateTimeSpiliter = DateTimeSpiliter.slash)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = MessageTypeResult.Success;
            try
            {
                PersianCalendar pc = new PersianCalendar();
                string year = pc.GetYear(dateTime).ToString();
                string month = pc.GetMonth(dateTime).ToString();
                string day = pc.GetDayOfMonth(dateTime).ToString();
                string result = "";
                #region Spilitter

                char spiliter = inputDateTimeSpiliter switch
                {
                    DateTimeSpiliter.slash => '/',
                    DateTimeSpiliter.back_slash => '\\',
                    DateTimeSpiliter.colon => ':',
                    DateTimeSpiliter.dash => '-',
                    DateTimeSpiliter.dot => '.',
                    DateTimeSpiliter.semi => ',',
                    DateTimeSpiliter.semicolon => ';',
                    DateTimeSpiliter.star => '*',
                    DateTimeSpiliter.underline => '_',
                    _ => '/'
                };
                //return $"{day}{spiliter}{month}{spiliter}{year}";
                switch (outputDateFormat)
                {
                    case DateTimeFormat.dd_mm_yyyy:
                        result = $"{day}{spiliter}{month}{spiliter}{year}";
                        break;
                    case DateTimeFormat.mm_dd_yyyy:
                        result = $"{month}{spiliter}{day}{spiliter}{year}";
                        break;
                    case DateTimeFormat.yyyy_dd_mm:
                        result = $"{year}{spiliter}{day}{spiliter}{month}";
                        break;
                    case DateTimeFormat.yyyy_mm_dd:
                        result = $"{year}{spiliter}{month}{spiliter}{day}";
                        break;
                    case DateTimeFormat.dd_yyyy_mm:
                        result = $"{day}{spiliter}{year}{spiliter}{month}";
                        break;
                    case DateTimeFormat.mm_yyyy_dd:
                        result = $"{month}{spiliter}{year}{spiliter}{day}";
                        break;
                    default:
                        result = $"{year}{spiliter}{month}{spiliter}{day}";
                        break;

                }

                #endregion
                result = $"{year}{spiliter}{month}{spiliter}{day}";
                return new HtmlString(result);
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public static DateTime ConvertPersianDateToDateTime(
            string perisanDateTime,
            DateTimeFormat inputDateTimeFormat,
            DateTimeSpiliter inputDateTimeSpiliter = DateTimeSpiliter.slash)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            resultStatusOperation.Message = "Message";
            try
            {
                if (String.IsNullOrEmpty(perisanDateTime))
                {
                    return DateTime.Now;
                }

                #region Spilitter

#pragma warning disable CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
                char spiliter = inputDateTimeSpiliter switch
                {
                    DateTimeSpiliter.slash => '/',
                    DateTimeSpiliter.back_slash => '\\',
                    DateTimeSpiliter.colon => ':',
                    DateTimeSpiliter.dash => '-',
                    DateTimeSpiliter.dot => '.',
                    DateTimeSpiliter.semi => ',',
                    DateTimeSpiliter.semicolon => ';',
                    DateTimeSpiliter.star => '*',
                    DateTimeSpiliter.underline => '_',

                };
#pragma warning restore CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.

                #endregion

                PersianCalendar persianCalendar = new PersianCalendar();
                string[] date = perisanDateTime.Split(spiliter);
                switch (inputDateTimeFormat)
                {
                    case DateTimeFormat.dd_mm_yyyy:
                        {
                            DateTime dateTime = persianCalendar.ToDateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]), 0, 0, 0, 0);
                            return dateTime;
                        }
                    case DateTimeFormat.mm_dd_yyyy:
                        {
                            DateTime dateTime = persianCalendar.ToDateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), 0, 0, 0, 0);
                            return dateTime;
                        }
                    case DateTimeFormat.yyyy_dd_mm:
                        {
                            DateTime dateTime = persianCalendar.ToDateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), 0, 0, 0, 0);
                            return dateTime;
                        }
                    case DateTimeFormat.yyyy_mm_dd:
                        {
                            DateTime dateTime = persianCalendar.ToDateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2]), 0, 0, 0, 0);
                            return dateTime;
                        }
                    case DateTimeFormat.dd_yyyy_mm:
                        {
                            DateTime dateTime = persianCalendar.ToDateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]), 0, 0, 0, 0);
                            return dateTime;
                        }
                    case DateTimeFormat.mm_yyyy_dd:
                        {
                            DateTime dateTime = persianCalendar.ToDateTime(Convert.ToInt32(date[1]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2]), 0, 0, 0, 0);
                            return dateTime;
                        }
                }

                return DateTime.Now;
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);

            }

        }


        public static DateTime? ToConvertPersianDateToDateTimeNullable(
           this string perisanDateTime,
           DateTimeFormat inputDateTimeFormat = DateTimeFormat.yyyy_mm_dd,
           DateTimeSpiliter inputDateTimeSpiliter = DateTimeSpiliter.slash)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            resultStatusOperation.Message = "Message";
            try
            {

                if (String.IsNullOrEmpty(perisanDateTime))
                {
                    return null;
                }

                return DateTimeUtility.ToConvertPersianDateToDateTime(perisanDateTime, inputDateTimeFormat, inputDateTimeSpiliter);
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);

            }

        }


        public static DateTime ToConvertPersianDateToDateTime(
            this string perisanDateTime,
            DateTimeFormat inputDateTimeFormat = DateTimeFormat.yyyy_mm_dd,
            DateTimeSpiliter inputDateTimeSpiliter = DateTimeSpiliter.slash)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            resultStatusOperation.Message = "Message";
            try
            {

                if (String.IsNullOrEmpty(perisanDateTime))
                {
                    return DateTime.Now;
                }
                #region Spilitter

#pragma warning disable CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
                char spiliter = inputDateTimeSpiliter switch
                {
                    DateTimeSpiliter.slash => '/',
                    DateTimeSpiliter.back_slash => '\\',
                    DateTimeSpiliter.colon => ':',
                    DateTimeSpiliter.dash => '-',
                    DateTimeSpiliter.dot => '.',
                    DateTimeSpiliter.semi => ',',
                    DateTimeSpiliter.semicolon => ';',
                    DateTimeSpiliter.star => '*',
                    DateTimeSpiliter.underline => '_',

                };
#pragma warning restore CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.

                #endregion

                PersianCalendar persianCalendar = new PersianCalendar();
                string[] date = perisanDateTime.Split(spiliter);
                switch (inputDateTimeFormat)
                {
                    case DateTimeFormat.dd_mm_yyyy:
                        {
                            DateTime dateTime = persianCalendar.ToDateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]), 0, 0, 0, 0);
                            return dateTime;
                        }
                    case DateTimeFormat.mm_dd_yyyy:
                        {
                            DateTime dateTime = persianCalendar.ToDateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), 0, 0, 0, 0);
                            return dateTime;
                        }
                    case DateTimeFormat.yyyy_dd_mm:
                        {
                            DateTime dateTime = persianCalendar.ToDateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), 0, 0, 0, 0);
                            return dateTime;
                        }
                    case DateTimeFormat.yyyy_mm_dd:
                        {
                            DateTime dateTime = persianCalendar.ToDateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2]), 0, 0, 0, 0);
                            return dateTime;
                        }
                }

                return DateTime.Now;
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);

            }

        }
        public static async Task<DateTime> ToConvertPersianDateToDateTimeAsync(
           this string perisanDateTime,
           DateTimeFormat inputDateTimeFormat = DateTimeFormat.yyyy_mm_dd,
           DateTimeSpiliter inputDateTimeSpiliter = DateTimeSpiliter.slash)
        {
            return await Task.Run(() =>
            {

                ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
                resultStatusOperation.IsSuccessed = true;
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
                resultStatusOperation.Message = "Message";
                try
                {

                    if (String.IsNullOrEmpty(perisanDateTime))
                    {
                        return DateTime.Now;
                    }
                    #region Spilitter

#pragma warning disable CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
                    char spiliter = inputDateTimeSpiliter switch
                    {
                        DateTimeSpiliter.slash => '/',
                        DateTimeSpiliter.back_slash => '\\',
                        DateTimeSpiliter.colon => ':',
                        DateTimeSpiliter.dash => '-',
                        DateTimeSpiliter.dot => '.',
                        DateTimeSpiliter.semi => ',',
                        DateTimeSpiliter.semicolon => ';',
                        DateTimeSpiliter.star => '*',
                        DateTimeSpiliter.underline => '_',

                    };
#pragma warning restore CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.

                    #endregion

                    PersianCalendar persianCalendar = new PersianCalendar();
                    string[] date = perisanDateTime.Split(spiliter);
                    switch (inputDateTimeFormat)
                    {
                        case DateTimeFormat.dd_mm_yyyy:
                            {
                                DateTime dateTime = persianCalendar.ToDateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]), 0, 0, 0, 0);
                                return dateTime;
                            }
                        case DateTimeFormat.mm_dd_yyyy:
                            {
                                DateTime dateTime = persianCalendar.ToDateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), 0, 0, 0, 0);
                                return dateTime;
                            }
                        case DateTimeFormat.yyyy_dd_mm:
                            {
                                DateTime dateTime = persianCalendar.ToDateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), 0, 0, 0, 0);
                                return dateTime;
                            }
                        case DateTimeFormat.yyyy_mm_dd:
                            {
                                DateTime dateTime = persianCalendar.ToDateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2]), 0, 0, 0, 0);
                                return dateTime;
                            }
                    }

                    return DateTime.Now;


                }
                catch (Exception exception)
                {
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = "خطایی رخ داده است";
                    resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                    resultStatusOperation.ErrorException = exception;
                    throw new Exception("", exception);

                }
            });
        }

        public static IHtmlContent ToConvertPersianDateToDateTime(
            this IHtmlHelper helper,
            string perisanDateTime,
            DateTimeFormat inputDateTimeFormat,
            DateTimeSpiliter inputDateTimeSpiliter = DateTimeSpiliter.slash)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            resultStatusOperation.Message = "Message";
            try
            {

                if (String.IsNullOrEmpty(perisanDateTime))
                {
                    return new HtmlString(DateTime.Now.ToString());
                }
                #region Spilitter

#pragma warning disable CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
                char spiliter = inputDateTimeSpiliter switch
                {
                    DateTimeSpiliter.slash => '/',
                    DateTimeSpiliter.back_slash => '\\',
                    DateTimeSpiliter.colon => ':',
                    DateTimeSpiliter.dash => '-',
                    DateTimeSpiliter.dot => '.',
                    DateTimeSpiliter.semi => ',',
                    DateTimeSpiliter.semicolon => ';',
                    DateTimeSpiliter.star => '*',
                    DateTimeSpiliter.underline => '_',

                };
#pragma warning restore CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.

                #endregion

                PersianCalendar persianCalendar = new PersianCalendar();
                string[] date = perisanDateTime.Split(spiliter);
                switch (inputDateTimeFormat)
                {
                    case DateTimeFormat.dd_mm_yyyy:
                        {
                            DateTime dateTime = persianCalendar.ToDateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]), 0, 0, 0, 0);
                            return new HtmlString(dateTime.ToString());
                        }
                    case DateTimeFormat.mm_dd_yyyy:
                        {
                            DateTime dateTime = persianCalendar.ToDateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), 0, 0, 0, 0);
                            return new HtmlString(dateTime.ToString());
                        }
                    case DateTimeFormat.yyyy_dd_mm:
                        {
                            DateTime dateTime = persianCalendar.ToDateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), 0, 0, 0, 0);
                            return new HtmlString(dateTime.ToString());
                        }
                    case DateTimeFormat.yyyy_mm_dd:
                        {
                            DateTime dateTime = persianCalendar.ToDateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2]), 0, 0, 0, 0);
                            return new HtmlString(dateTime.ToString());
                        }
                }

                return new HtmlString(DateTime.Now.ToString());
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);

            }
        }
    }
}
