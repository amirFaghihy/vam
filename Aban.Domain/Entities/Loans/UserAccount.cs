using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Domain.Entities
{
    /// <summary>
    /// حسابهای مالی افراد
    /// </summary>
    public class UserAccount : BaseEntity<int>
    {
        #region ForeignKeys

        /// <summary>
        /// صاحب حساب
        /// </summary>
        [Display(Name = "صاحب حساب")]
#pragma warning disable CS8618
        public string AccountOwnerId { get; set; }

        /// <summary>
        /// صاحب حساب
        /// </summary>
        [ForeignKey("AccountOwnerId")]
        [Display(Name = "صاحب حساب")]
        public UserIdentity? AccountOwner { get; set; }

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
        /// شماره حساب
        /// </summary>
        [Display(Name = "شماره حساب")]
        public string? AccountNumber { get; set; }

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

}
