using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aban.Domain.Enumerations
{
    public class Enumeration
    {


        public enum ActionReturnType
        {
            View,
            PartialView,
            ComponentView,
            Json,
            RenderedCSHtml,
            StaticHTML
        }

        public enum ActionType
        {
            Index,
            Create_Get,
            Create_Post,
            Edit_Get,
            Edit_Post,

        }
        /// <summary>
        /// آخرین مدرک تحصیلی
        /// </summary>
        public enum LastDegree : byte
        {
            نامشخص,
            دیپلم,
            فوق_دیپلم,
            کارشناسی,
            کارشناسی_ارشد,
            دکترا,
        }

        /// <summary>
        /// حالت نمایش پیغام
        /// </summary>
        public enum MessageTypeResult
        {
            Success = 0,
            Danger = 1,
            Warning = 2,
            Info = 3,
        }

        /// <summary>
        /// نوع متدی که صدا زده شده است
        /// </summary>
        public enum MessageTypeActionMethod
        {
            Create = 0,
            EditGet,
            EditPost,
            CreateGet,
            CreatePost,
            Delete,
            Index,
        }

        /// <summary>
        /// نام رول هایی که در دیتابیس باید ذخیره شوند
        /// و بعنوان پروپرتی بالای متد و کلاسها براس سطح دسترسی استفاده میشوند
        /// </summary>
        public enum RoleName
        {
            [Display(Name = "کاربر معمولی")]
            User_Register,

            [Display(Name = "مدیر سایت")]
            Admin,

            [Display(Name = "مدیر اصلی سایت")]
            SuperAdmin,

            [Display(Name = "سرکاربر")]
            Foreman,

            [Display(Name = "منشی")]
            Clerk,

            [Display(Name = "خیّر")]
            Helper
        }


        /// <summary>
		/// نوع تاریخ انقضا
		/// </summary>
		public enum ExpireTimeType : byte
        {
            Minutes,
            Hours,
            Days,
            Weeks,
            Months,
            Years
        }


        public enum TypeOfLoginMethod
        {
            isLoginByUserNamePasword,
            isLoginOnlyUserName,
            isLoginByPhoneNumber,
            isLoginOnlyPhoneNumber,
        }


        /// <summary>
        /// نوع تاریخ 
        /// </summary>
        public enum DateTimeFormat : byte
        {
            dd_mm_yyyy,
            mm_dd_yyyy,
            yyyy_dd_mm,
            yyyy_mm_dd,
            dd_yyyy_mm,
            mm_yyyy_dd
        }


        public enum DateTimeSpiliter : byte
        {
            dot,
            underline,
            dash,
            slash,
            back_slash,
            star,
            semicolon,
            colon,
            semi
        }

        /// <summary>
        /// نام تمامی بانکهای ایران
        /// </summary>
        public enum BankName : byte
        {
            بانک_ملی_ایران,
            بانک_سپه,
            بانک_صنعت_و_معدن,
            بانک_کشاورزی,
            بانک_مسکن,
            بانک_توسعه_صادرات_ایران,
            بانک_توسعه_تعاون,
            پست_بانک_ایران,
            بانک_اقتصاد_نوین,
            بانک_پارسیان,
            بانک_کارآفرین,
            بانک_سامان,
            بانک_سینا,
            بانک_خاورمیانه,
            بانک_شهر,
            بانک_دی,
            بانک_صادرات,
            بانک_ملت,
            بانک_تجارت,
            بانک_رفاه,
            بانک_حکمت_ایرانیان,
            بانک_گردشگردی,
            بانک_ایران_زمین,
            بانک_قوامین,
            بانک_انصار,
            بانک_سرمایه,
            بانک_پاسارگاد,
            بانک_قرض_الحسنه_مهر_ایران,
            بانک_قرض_الحسنه_مهر_رسالت,
        }

        /// <summary>
        /// نوع تراکنش
        /// </summary>
        public enum TransactionType : byte
        {
            واریز,
            برداشت,
            پرداخت_قسط,
        }

        /// <summary>
        /// روش تراکنش
        /// </summary>
        public enum TransactionMethod : byte
        {
            نقدی,
            کارت_به_کارت,
            واریز_به_حساب,
            کارتخوان_مؤسسه,
            کسر_از_حقوق,
            پرداخت_از_طریق_خود_مؤسسه,
            برداشت_سیستمی
        }
    }
}
