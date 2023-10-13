using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Aban.Domain.Entities
{
    /// <summary>
    /// حسابهای مؤسسه
    /// </summary>
    public class CharityAccount : BaseEntity<int>
    {
        #region ForeignKeys

        /// <summary>
        /// ثبت کننده
        /// </summary>
        [Display(Name = "ثبت کننده")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string UserIdentityId { get; set; }

        /// <summary>
        /// ثبت کننده
        /// </summary>
        [ForeignKey("UserIdentityId")]
        [Display(Name = "ثبت کننده")]
        public UserIdentity UserIdentity { get; set; }

        #endregion

        /// <summary>
        /// نام بانک
        /// </summary>
        [Display(Name ="نام بانک")]
        public BankName BankName { get; set; }

        /// <summary>
        /// عنوان حساب
        /// </summary>
        [Display(Name = "عنوان حساب")]
        [Required(ErrorMessage ="{0} را وارد کنید")]
        public string Title { get; set; }

        /// <summary>
        /// شماره حساب/کارت
        /// </summary>
        [Display(Name = "شماره حساب/کارت")]
        [Required(ErrorMessage ="{0} را وارد کنید")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// فعال ؟
        /// </summary>
        [Display(Name = "فعال ؟")]
        public bool IsVisible { get; set; }

        

        /// <summary>
        /// توضیحات
        /// </summary>
        [Display(Name = "توضیحات")]
        [Required(ErrorMessage ="{0} را وارد کنید")]
        public string Description { get; set; }

        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        [Display(Name = "تاریخ ثبت")]
        public DateTime RegisterDate { get; set; }
    }

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

}
