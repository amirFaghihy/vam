using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Aban.Domain.Entities
{
    /// <summary>
    /// کسورات
    /// </summary>
    public class CharityDeducation : BaseEntity<int>
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



        /// <summary>
        /// دریافت کننده
        /// </summary>
        [Display(Name = "دریافت کننده")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string UserIdentityReciverId { get; set; }

        /// <summary>
        /// دریافت کننده
        /// </summary>
        [ForeignKey("UserIdentityReciverId")]
        [Display(Name = "دریافت کننده")]
        public UserIdentity UserIdentityReciver { get; set; }


        #endregion

        /// <summary>
        /// عنوان
        /// </summary>
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Title { get; set; }

        /// <summary>
        /// مقدار
        /// </summary>
        [Display(Name = "مقدار")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        [RegularExpression(@"^\d(.*\d)?$", ErrorMessage ="{0} فقط میتواند عدد باشد")]
        public double Amount { get; set; }

        /// <summary>
        /// شماره حساب/کارت
        /// </summary>
        [Display(Name = "شماره حساب/کارت")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string AccountNumber { get; set; }


        /// <summary>
        /// شرح سند
        /// </summary>
        [Display(Name = "شرح سند")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Description { get; set; }


        /// <summary>
        /// تاریخ اعمال
        /// </summary>
        [Display(Name = "تاریخ اعمال")]
        public DateTime TimeForAction { get; set; }


        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        [Display(Name = "تاریخ ثبت")]
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// اعمال شده؟
        /// </summary>
        [Display(Name = "اعمال شده؟")]
        public bool? IsDone { get; set; }
    }
}
