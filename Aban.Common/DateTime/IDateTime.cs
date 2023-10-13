using System;
using System.Globalization;

namespace Aban.Common
{

    public class IDateTime
    {
        #region Field & Properties

        #region Persian Calendar

        /// <summary>
        /// <para>شی پایه تقویم شمسی</para>
        /// </summary>
        private PersianCalendar pc = new PersianCalendar();

        #endregion

        #region base Date Time

        /// <summary>
        /// <para>تاریخ میلادی که شی جاری با آن تنظیم شده است</para>
        /// <para>اطلاعات تاریخ شمسی که در اختیار کاربر قرار می گیرد، معادل با این تاریخ است</para>
        /// <para>برای تنظیم این خاصیت از متد SetBaseDateTime استفاده کنید</para>
        /// </summary>
        private System.DateTime baseDateTime = System.DateTime.Now;

        //چرا این خاصیت دستیاب ست ندارد و مقدار دهی فیلد مربوطه با متد
        //SetBaseDateTime
        //صورت می گیرد؟
        //به این خاطر که این شی پیش فرض با تاریخ جاری مقداردهی می شود و 
        //بیشتر تاریخ جاری مورد نیاز است
        //این امکان گذاشته شده که این فیلد مقدارش تغییر کند اما اگر در دیزاینر
        //این امکان می بود امکان بوجود امدن اشتباه توسط کاربر بوجود
        //می امد با تغییر دادن اشتباهی مقدار خاصیت و ثبت ان در کد دیزاینر
        //برای جلوگیری از بروز خطا از تغییر این خاصیت در زمان طراحی جلوگیری شده
        /// <summary>
        /// <para>تاریخ میلادی که شی جاری با آن تنظیم شده است</para>
        /// <para>اطلاعات تاریخ شمسی که در اختیار کاربر قرار می گیرد، معادل با این تاریخ است</para>
        /// <para>برای تنظیم این خاصیت از متد SetBaseDateTime استفاده کنید</para>
        /// </summary>
        public System.DateTime BaseDateTime
        {
            get { return baseDateTime; }
        }

        #endregion

        #region Date Time

        /// <summary>
        /// <para>تاریخ هجری شمسی و ساعت به قالب زیر</para>
        /// <para>1387/06/22 03:25 ب.ظ</para>
        /// <para>تابع ToString هم همین مقدار را بر می گرداند</para>
        /// </summary>
        public virtual string DateTime
        {
            get
            {
                return pc.GetYear(baseDateTime).ToString()
                + "/"
                + (pc.GetMonth(baseDateTime) < 10
                    ? ("0" + pc.GetMonth(baseDateTime).ToString())
                    : pc.GetMonth(baseDateTime).ToString())
                + "/"
                + (pc.GetDayOfMonth(baseDateTime) < 10
                    ? ("0" + pc.GetDayOfMonth(baseDateTime).ToString())
                    : pc.GetDayOfMonth(baseDateTime).ToString())
                + " "
                + string.Format("{0:HH:mm}", baseDateTime);
            }
        }

        #endregion

        #region Date

        /// <summary>
        /// <para>تاریخ هجری شمسی به قالب زیر</para>
        /// <para>1387/06/22</para>
        /// </summary>
        public virtual string Date
        {
            get
            {
                return pc.GetYear(baseDateTime).ToString()
                + "/"
                + (pc.GetMonth(baseDateTime) < 10
                    ? ("0" + pc.GetMonth(baseDateTime).ToString())
                    : pc.GetMonth(baseDateTime).ToString())
                + "/"
                + (pc.GetDayOfMonth(baseDateTime) < 10
                    ? ("0" + pc.GetDayOfMonth(baseDateTime).ToString())
                    : pc.GetDayOfMonth(baseDateTime).ToString());
            }
        }

        #endregion

        #region Time

        /// <summary>
        /// <para>زمان</para>
        /// </summary>
        public virtual string Time
        {
            get
            {
                return baseDateTime.ToShortTimeString();
            }
        }

        #endregion

        #region StartWeek

        public IDateTime StartWeek
        {
            get
            {
                string tmpDayOfWeek = this.DayOfWeek;

                if (tmpDayOfWeek == EnumCollection.DayOfWeek_fr.شنبه.ToString())
                {
                    return this;
                }
                else if (tmpDayOfWeek == EnumCollection.DayOfWeek_fr.یک_شنبه.ToString().Replace('_', ' '))
                {
                    return this.AddDays(-1);
                }
                else if (tmpDayOfWeek == EnumCollection.DayOfWeek_fr.دو_شنبه.ToString().Replace('_', ' '))
                {
                    return this.AddDays(-2);
                }
                else if (tmpDayOfWeek == EnumCollection.DayOfWeek_fr.سه_شنبه.ToString().Replace('_', ' '))
                {
                    return this.AddDays(-3);
                }
                else if (tmpDayOfWeek == EnumCollection.DayOfWeek_fr.چهار_شنبه.ToString().Replace('_', ' '))
                {
                    return this.AddDays(-4);
                }
                else if (tmpDayOfWeek == EnumCollection.DayOfWeek_fr.پنج_شنبه.ToString().Replace('_', ' '))
                {
                    return this.AddDays(-5);
                }
                else if (tmpDayOfWeek == EnumCollection.DayOfWeek_fr.جمعه.ToString())
                {
                    return this.AddDays(-6);
                }
                else
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return null;
#pragma warning restore CS8603 // Possible null reference return.
                }
            }
        }

        #endregion

        #region EndWeek

        public IDateTime EndWeek
        {
            get
            {
                string tmpDayOfWeek = this.DayOfWeek;

                if (tmpDayOfWeek == EnumCollection.DayOfWeek_fr.شنبه.ToString())
                {
                    return this.AddDays(6);
                }
                else if (tmpDayOfWeek == EnumCollection.DayOfWeek_fr.یک_شنبه.ToString().Replace('_', ' '))
                {
                    return this.AddDays(5);
                }
                else if (tmpDayOfWeek == EnumCollection.DayOfWeek_fr.دو_شنبه.ToString().Replace('_', ' '))
                {
                    return this.AddDays(4);
                }
                else if (tmpDayOfWeek == EnumCollection.DayOfWeek_fr.سه_شنبه.ToString().Replace('_', ' '))
                {
                    return this.AddDays(3);
                }
                else if (tmpDayOfWeek == EnumCollection.DayOfWeek_fr.چهار_شنبه.ToString().Replace('_', ' '))
                {
                    return this.AddDays(2);
                }
                else if (tmpDayOfWeek == EnumCollection.DayOfWeek_fr.پنج_شنبه.ToString().Replace('_', ' '))
                {
                    return this.AddDays(1);
                }
                else if (tmpDayOfWeek == EnumCollection.DayOfWeek_fr.جمعه.ToString())
                {
                    return this;
                }
                else
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return null;
#pragma warning restore CS8603 // Possible null reference return.
                }
            }
        }

        #endregion

        #region Day Of Week

        /// <summary>
        /// روز در هفته مثل جمعه
        /// </summary>
        public virtual string DayOfWeek
        {
            get
            {
                return ConvertDay(baseDateTime.DayOfWeek);
            }
        }

        #endregion

        #region Day Of Month

        /// <summary>
        /// <para>روز در ماه مثل 21</para>
        /// </summary>
        public virtual int DayOfMonth
        {
            get
            {
                return pc.GetDayOfMonth(baseDateTime);
            }
        }

        #endregion

        #region Day Of Year

        /// <summary>
        /// <para>روز از سال مثل 177</para>
        /// </summary>
        public virtual int DayOfYear
        {
            get
            {
                return pc.GetDayOfYear(baseDateTime);
            }
        }

        #endregion

        #region Month Name

        /// <summary>
        /// نام ماه مثل اردیبهشت
        /// </summary>
        public virtual string MonthName
        {
            get
            {
                return ConvertMonth(pc.GetMonth(baseDateTime));
            }
        }

        #endregion

        #region Month Number

        /// <summary>
        ///عدد ماه مثل 2
        /// </summary>

        public virtual int MonthNumber
        {
            get
            {
                return pc.GetMonth(baseDateTime);
            }
        }

        #endregion

        #region Year

        /// <summary>
        ///عدد سال
        /// </summary>

        public virtual int Year
        {
            get
            {
                return pc.GetYear(baseDateTime);
            }
        }

        #endregion

        #region Hour

        /// <summary>
        ///ساعت
        /// </summary>

        public virtual int Hour
        {
            get
            {
                return pc.GetHour(baseDateTime);
            }
        }

        #endregion

        #region Minute

        /// <summary>
        ///دقیقه
        /// </summary>

        public virtual int Minute
        {
            get
            {
                return pc.GetMinute(baseDateTime);
            }
        }

        #endregion

        #region Second

        /// <summary>
        ///ثانیه
        /// </summary>

        public virtual int Second
        {
            get
            {
                return pc.GetSecond(baseDateTime);
            }
        }

        #endregion

        #region Milliseconds

        /// <summary>
        ///میلی ثانیه
        /// </summary>

        public virtual double Milliseconds
        {
            get
            {
                return pc.GetMilliseconds(baseDateTime);
            }
        }

        #endregion

        #region Special Format 1

        /// <summary>
        ///<para>قالب خاص تاریخ مثل</para>
        ///<para>شنبه 23 شهریور 1387</para>
        /// </summary>

        public virtual string SpecialFormat1
        {
            get
            {
                return this.DayOfWeek
                    + " "
                    + this.DayOfMonth
                    + " "
                    + this.MonthName
                    + " "
                    + this.Year.ToString();
            }
        }

        #endregion

        #region Special Format 2

        /// <summary>
        ///<para>قالب خاص تاریخ مثل</para>
        ///<para>23 شهریور 1387</para>
        /// </summary>

        public virtual string SpecialFormat2
        {
            get
            {
                return this.DayOfMonth
                    + " "
                    + this.MonthName
                    + " "
                    + this.Year.ToString();
            }
        }

        #endregion

        #region Special Format 3

        /// <summary>
        ///<para>قالب خاص تاریخ مثل</para>
        ///<para>شنبه 23 شهریور</para>
        /// </summary>

        public virtual string SpecialFormat3
        {
            get
            {
                return this.DayOfWeek
                    + " "
                    + this.DayOfMonth
                    + " "
                    + this.MonthName;
            }
        }

        #endregion

        #region MonthCapacity

        public int MonthCapacity
        {
            get
            {
                if ((this.MonthNumber >= 1) && (this.MonthNumber <= 6))
                {
                    return 31;
                }
                if ((this.MonthNumber >= 7) && (this.MonthNumber <= 11))
                {
                    return 30;
                }
                else
                {
                    return 29;
                }
            }
        }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// سازنده پیش فرض
        /// </summary>
        /// <param name="baseDateTime"></param>
        public IDateTime()
        {
            this.baseDateTime = System.DateTime.Now;
        }


        /// <summary>
        /// این سازنده اطلاعات تاریخ و زمان را دریافت و شی را می سازد
        /// </summary>
        /// <param name="year">سال</param>
        /// <param name="month">ماه</param>
        /// <param name="day">روز</param>
        /// <param name="hour">ساعت</param>
        /// <param name="minute">دقیقه</param>
        /// <param name="second">ثانیه</param>
        /// <param name="millisecond">میلی ثانیه</param>
        public IDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            try
            {
                baseDateTime = pc.ToDateTime(year, month, day, hour, minute, second, millisecond);
            }
            catch (Exception ex)
            {
                throw new Exception("ایجاد شی تاریخ هجری شمسی با شکست مواجه شد. لطفا پارامترهای ارسال شده را بررسی کنید." + "\n" + ex.Message);
            }
        }

        /// <summary>
        /// <para>با این سازنده می توانید شی با تاریخ دلخواه بسازید</para>
        /// </summary>
        /// <param name="gregorianDate">تاریخ میلادی معادل با تاریخی که می خواهید</param>
        public IDateTime(DateTime baseDateTime)
        {
            if (baseDateTime == System.DateTime.MinValue)
            {
                this.baseDateTime = pc.MinSupportedDateTime;
            }
            else if (baseDateTime == System.DateTime.MaxValue)
            {
                this.baseDateTime = pc.MaxSupportedDateTime;
            }
            else
            {
                this.baseDateTime = baseDateTime;
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// روز رو به میلادی می گیره و به شمسی بر می گردونه
        /// </summary>
        /// <param name="gregorianDay">روز به میلادی</param>
        /// <returns>روز به هجری شمسی</returns>
        private string ConvertDay(System.DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case System.DayOfWeek.Friday:
                    return EnumCollection.DayOfWeek_fr.جمعه.ToString();
                case System.DayOfWeek.Saturday:
                    return EnumCollection.DayOfWeek_fr.شنبه.ToString();
                case System.DayOfWeek.Sunday:
                    return EnumCollection.DayOfWeek_fr.یک_شنبه.ToString().Replace('_', ' ');
                case System.DayOfWeek.Monday:
                    return EnumCollection.DayOfWeek_fr.دو_شنبه.ToString().Replace('_', ' ');
                case System.DayOfWeek.Tuesday:
                    return EnumCollection.DayOfWeek_fr.سه_شنبه.ToString().Replace('_', ' ');
                case System.DayOfWeek.Wednesday:
                    return EnumCollection.DayOfWeek_fr.چهار_شنبه.ToString().Replace('_', ' ');
                case System.DayOfWeek.Thursday:
                    return EnumCollection.DayOfWeek_fr.پنج_شنبه.ToString().Replace('_', ' ');
                default:
                    return "";
            }
        }

        /// <summary>
        /// <para>شماره ماه رو به هجری شمسی می گیره نام ماه رو به هجری شمسی بر می گردونه</para>
        /// </summary>
        /// <param name="numMonth">شماره ماه به هجری شمسی</param>
        /// <returns>نام ماه به هجری شمسی</returns>
        private string ConvertMonth(int month)
        {
            switch (month)
            {
                case 1:
                    return "فروردین";
                case 2:
                    return "اردیبهشت";
                case 3:
                    return "خرداد";
                case 4:
                    return "تیر";
                case 5:
                    return "مرداد";
                case 6:
                    return "شهریور";
                case 7:
                    return "مهر";
                case 8:
                    return "آبان";
                case 9:
                    return "آذر";
                case 10:
                    return "دی";
                case 11:
                    return "بهمن";
                case 12:
                    return "اسفند";
                default:
                    return "";
            }
        }

        /// <summary>
        /// <para>برای مقدار دهی به تاریخ میلادی که تاریخ شمسی جاری بر اساس آن ساخته می شود</para>
        /// </summary>
        /// <param name="baseDateTime">تاریخ میلادی</param>
        public void SetBaseDateTime(System.DateTime baseDateTime)
        {
            if (baseDateTime == System.DateTime.MinValue)
            {
                this.baseDateTime = pc.MinSupportedDateTime;
            }
            else if (baseDateTime == System.DateTime.MaxValue)
            {
                this.baseDateTime = pc.MaxSupportedDateTime;
            }
            else
            {
                this.baseDateTime = baseDateTime;
            }
        }

        #region Add ...

        /// <summary>
        /// افزودن روز به تاریخ هجری شمسی جاری
        /// </summary>
        /// <param name="days">تعداد روزهایی که باید افزوده شود</param>
        /// <returns>تاریخ هجری شمسی جدید</returns>
        public virtual IDateTime AddDays(int days)
        {
            System.DateTime dt = pc.AddDays(baseDateTime, days);
            return new IDateTime(dt);
        }

        /// <summary>
        /// افزودن ماه به تاریخ هجری شمسی جاری
        /// </summary>
        /// <param name="days">تعداد ماه هایی که باید افزوده شود</param>
        /// <returns>تاریخ هجری شمسی جدید</returns>
        public virtual IDateTime AddMonths(int months)
        {
            System.DateTime dt = pc.AddMonths(baseDateTime, months);
            return new IDateTime(dt);
        }

        /// <summary>
        /// افزودن سال به تاریخ هجری شمسی جاری
        /// </summary>
        /// <param name="days">تعداد سال هایی که باید افزوده شود</param>
        /// <returns>تاریخ هجری شمسی جدید</returns>
        public virtual IDateTime AddYears(int years)
        {
            System.DateTime dt = pc.AddYears(baseDateTime, years);
            return new IDateTime(dt);
        }

        /// <summary>
        /// افزودن ساعت به تاریخ هجری شمسی جاری
        /// </summary>
        /// <param name="days">تعداد ساعت هایی که باید افزوده شود</param>
        /// <returns>تاریخ هجری شمسی جدید</returns>
        public virtual IDateTime AddHours(int hours)
        {
            System.DateTime dt = pc.AddHours(baseDateTime, hours);
            return new IDateTime(dt);
        }

        /// <summary>
        /// افزودن دقیقه به تاریخ هجری شمسی جاری
        /// </summary>
        /// <param name="days">تعداد دقیقه هایی که باید افزوده شود</param>
        /// <returns>تاریخ هجری شمسی جدید</returns>
        public virtual IDateTime AddMinutes(int minutes)
        {
            System.DateTime dt = pc.AddMinutes(baseDateTime, minutes);
            return new IDateTime(dt);
        }

        /// <summary>
        /// افزودن ثانیه به تاریخ هجری شمسی جاری
        /// </summary>
        /// <param name="days">تعداد ثانیه هایی که باید افزوده شود</param>
        /// <returns>تاریخ هجری شمسی جدید</returns>
        public virtual IDateTime AddSeconds(int seconds)
        {
            System.DateTime dt = pc.AddSeconds(baseDateTime, seconds);
            return new IDateTime(dt);
        }

        /// <summary>
        /// افزودن هفته به تاریخ هجری شمسی جاری
        /// </summary>
        /// <param name="days">تعداد هفته هایی که باید افزوده شود</param>
        /// <returns>تاریخ هجری شمسی جدید</returns>
        public virtual IDateTime AddWeeks(int weeks)
        {
            System.DateTime dt = pc.AddWeeks(baseDateTime, weeks);
            return new IDateTime(dt);
        }

        #endregion

        /// <summary>
        /// مقایسه دو تاریخ هجری شمسی
        /// </summary>
        /// <param name="IPersianCalendar">تاریخ شمسی که باید با این شی مقایسه شود</param>
        /// <param name="option">
        /// <para>تنظیمات مقایسه</para>
        /// <para>در حالتی که زمان هم در مقایسه تاثیر داشته باشد اگر هر دو تاریخ مربوط به یک روز باشند انگاه زمان تعیین می کند دو تاریخ چه وضعیتی نسبت به هم دارند</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>EnumCollection.CompareResult.Equal</para>
        /// <para>با تاریخ ارسال شده برابر هستم</para>
        /// <para>....................................</para>
        /// <para>EnumCollection.CompareResult.GreaterThan</para>
        /// <para>از تاریخ ارسال شده بزرگتر هستم</para>
        /// <para>....................................</para>
        /// <para>EnumCollection.CompareResult.LessThan</para>
        /// <para>از تاریخ ارسال شده کوچکتر هستم</para>
        /// <para>....................................</para>
        /// <para>EnumCollection.CompareResult.Non</para>
        /// <para>نا مشخص</para>
        /// </returns>
        public virtual EnumCollection.CompareResult CompareTo(IDateTime hrPersianCalendar, EnumCollection.CompareOption option)
        {
            int rst = -2;

            if (option == EnumCollection.CompareOption.WithTime)
            {
                rst = baseDateTime.CompareTo(hrPersianCalendar.baseDateTime);
            }
            else if (option == EnumCollection.CompareOption.WithoutTime)
            {
                rst = baseDateTime.Date.CompareTo(hrPersianCalendar.baseDateTime.Date);
            }

            if (rst == 0)
            {
                return EnumCollection.CompareResult.Equal;
            }
            else if (rst > 0)
            {
                return EnumCollection.CompareResult.GreaterThan;
            }
            else if (rst < 0)
            {
                return EnumCollection.CompareResult.LessThan;
            }
            else
            {
                return EnumCollection.CompareResult.Non;
            }
        }

        /// <summary>
        /// تفاضل با تاریخ ارسال شده
        /// </summary>
        /// <param name="IPersianCalendar">تاریخی که باید تفاضلش با این تاریخ بدست آید</param>
        /// <returns>اطلاعات تفاضل</returns>
        public virtual TimeSpan Subtract(IDateTime hrPersianCalendar)
        {
            return baseDateTime.Subtract(hrPersianCalendar.baseDateTime);
        }

        public override string ToString()
        {
            return DateTime;
        }

        #endregion

        #region Static Method
        /// <summary>
        /// Get DateTime as parameter and convert to solar date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetPersianDate(System.DateTime? date)
        {
            var pc = new PersianCalendar();
            return date != null
                ? $"{pc.GetYear((System.DateTime)date)}/{pc.GetMonth((System.DateTime)date)}/{pc.GetDayOfMonth((System.DateTime)date)}"
                : "";
        }


        /// <summary>
        /// تبدیل تاریخ هجری شمسی به میلادی
        /// </summary>
        /// <param name="year">عدد سال</param>
        /// <param name="month">عدد ماه</param>
        /// <param name="day">عدد روز</param>
        /// <param name="hour">ساعت</param>
        /// <param name="minute">دقیقه</param>
        /// <param name="second">ثانیه</param>
        /// <param name="millisecond">میلی ثانیه</param>
        /// <returns>تاریخ میلادی</returns>
        public static System.DateTime PersianToGregorian(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            PersianCalendar p = new PersianCalendar();
            DateTime dt;

            try
            {
                dt = p.ToDateTime(year, month, day, hour, minute, second, millisecond);
            }
            catch (Exception ex)
            {
                throw new Exception("ایجاد شی تاریخ هجری شمسی با شکست مواجه شد. لطفا پارامترهای ارسال شده را بررسی کنید." + "\n" + ex.Message);
            }

            return dt;
        }

        /// <summary>
        /// تبدیل تاریخ هجری شمسی به میلادی
        /// </summary>
        /// <param name="persianCalendar">تاریخ شمسی</param>
        /// <returns>تاریخ میلادی</returns>
        public static System.DateTime PersianToGregorian(IDateTime hrPersianCalendar)
        {
            PersianCalendar p = new PersianCalendar();
            return p.ToDateTime(hrPersianCalendar.Year, hrPersianCalendar.MonthNumber, hrPersianCalendar.DayOfMonth, hrPersianCalendar.Hour, hrPersianCalendar.Minute, hrPersianCalendar.Second, (int)hrPersianCalendar.Milliseconds);
        }

        //@@
        //قالبش با re چک بشه
        /// <summary>
        /// بررسی میکند که فرمت و مقادیر وارد شده در تاریخ معتبر باشد
        /// فرمت مورد قبول 23/01/1386
        /// </summary>
        /// <param name="date">تاریخ به صورت رشته</param>
        /// <returns>اگر خالی باشد تاریخ معتبر است در غیر این صورت حاوی پیام متن خطا</returns>
        public static DateTime ChekDate(string date, string time)
        {
            string msg = "";
            //if (date == "" || date.Trim() == "/  /")
            //{
            //     return msg;
            //}
            //else if (time == "" || time.Trim() == ":")
            //{
            //    return msg;
            //}
            //else
            {

                int day = 0, month = 0, year = 0, h = 0, m = 0;
                bool space = false;
                bool kabise = false;

                foreach (char ch in date.ToCharArray())
                {
                    if (ch == ' ')
                    {
                        space = true;
                        break;
                    }
                }

                if ((date.Length > 10 && date.Length < 8) || space == true)
                {
                    msg = "لطفا تاریخ را کامل وارد کنید";
                }
                else
                {
                    try
                    {
                        year = Convert.ToInt32(date.Substring(0, 4));
                        if (date[6] == ('/'))
                        {
                            month = Convert.ToInt32(date.Substring(5, 1));
                            day = Convert.ToInt32(date.Substring(7));
                        }
                        else
                        {
                            month = Convert.ToInt32(date.Substring(5, 2));
                            day = Convert.ToInt32(date.Substring(8));
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception("فرمت تاریخ وارد شده اشتباه است");
                    }
                    try
                    {
                        var t = time.IndexOf(':');
                        h = Convert.ToInt32(time.Substring(0, t));
                        m = Convert.ToInt32(time.Substring(t + 1));
                    }
                    catch (Exception)
                    {
                        throw new Exception("فرمت ساعت وارد شده اشتباه است");
                    }

                    for (int i = 584; i <= year; i += 4)
                    {
                        if (year == i)
                        {
                            kabise = true;
                        }
                    }

                    if (!(year >= 1001 && month <= 2000))
                    {
                        msg += "عدد وارد شده برای سال باید بین 1001 تا 2000 باشد" + "\n";
                    }
                    else if (!(month >= 1 && month <= 12))
                    {
                        msg += "عدد وارد شده برای ماه باید بین 1 تا 12 باشد" + "\n";
                    }
                    else if (month >= 1 && month <= 6)
                    {
                        if (!(day >= 1 && day <= 31))
                        {
                            msg += "برای ماه های فروردین تا شهریور روز باید در محدوده یک تا سی و یک باشد" + "\n";
                        }
                    }
                    else if (month >= 7 && month <= 11)
                    {
                        if (!(day >= 1 && day <= 30))
                        {
                            msg += "برای ماه های مهر تا بهمن روز باید در محدوده یک تا سی باشد" + "\n";
                        }
                    }
                    else if (month == 12)
                    {
                        if (kabise == true)
                        {
                            if (!(day >= 1 && day <= 30))
                            {
                                msg += "برای ماه اسفند در سال کبیسه روز باید در محدوده یک تا سی باشد" + "\n";
                            }
                        }
                        else
                        {
                            if (!(day >= 1 && day <= 29))
                            {
                                msg += "برای ماه اسفند در سال غیرکبیسه روز باید در محدوده یک تا بیست و نه باشد" + "\n";
                            }
                        }
                    }
                }



                if (!string.IsNullOrWhiteSpace(msg))
                    throw new Exception(msg);
                return PersianToGregorian(year, month, day, h, m, 0, 0);
            }
        }

        #endregion
    }


    public class EnumCollection
    {
        /// <summary>
        /// مقایسه مقادیر
        /// </summary>
        public enum CompareResult
        {
            /// <summary>
            /// نا مشخص
            /// </summary>
            Non,
            /// <summary>
            /// مساوی
            /// </summary>
            Equal,
            /// <summary>
            /// کمتر، کوچکتر از
            /// </summary>
            LessThan,
            /// <summary>
            /// بیشتر، بزرگتر از
            /// </summary>
            GreaterThan,
        }

        public enum CompareOption
        {
            /// <summary>
            /// زمان در مقایسه تاثیر داشته باشد
            /// </summary>
            WithTime,
            /// <summary>
            /// زمان در مقایسه تاثیر نداشته باشد
            /// </summary>
            WithoutTime,
        }

        public enum DayOfWeek_fr : byte
        {
            شنبه,
            یک_شنبه,
            دو_شنبه,
            سه_شنبه,
            چهار_شنبه,
            پنج_شنبه,
            جمعه,
        }
    }
}
