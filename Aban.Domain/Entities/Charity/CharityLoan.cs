using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Aban.Domain.Entities
{
    /// <summary>
    /// وام
    /// </summary>
    public class CharityLoan : BaseEntity<int>
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
        public UserIdentity? UserIdentity { get; set; }

        /// <summary>
        /// وام گیرنده
        /// </summary>
        [Display(Name = "وام گیرنده")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Required(ErrorMessage ="{0} را انتخاب کنید")]
        public string LoanReceiverId { get; set; }

        /// <summary>
        /// وام گیرنده
        /// </summary>
        [ForeignKey("LoanReceiverId")]
        [Display(Name = "وام گیرنده")]
        public UserIdentity? LoanReceiver { get; set; }


        #endregion


        /// <summary>
        /// مبلغ وام
        /// </summary>
        [Display(Name = "مبلغ وام")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        [RegularExpression(@"^\d(.*\d)?$", ErrorMessage = "{0} فقط میتواند عدد باشد")]
        public float LoanAmount { get; set; } = 0;


        /// <summary>
        /// درصد سود وام
        /// </summary>
        [Display(Name = "درصد سود وام")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public byte PercentSalary { get; set; } = 0;

        /// <summary>
        /// شماره حساب/کارت واریزی
        /// </summary>
        [Display(Name = "شماره حساب/کارت واریزی")]
        public string? AccountNumber { get; set; }


        /// <summary>
        /// شرح سند
        /// </summary>
        [Display(Name = "شرح سند")]
        [Required(ErrorMessage ="{0} را وارد کنید")]
        public string Description { get; set; }


        /// <summary>
        /// تاریخ شروع پرداخت
        /// </summary>
        [Display(Name = "تاریخ شروع پرداخت")]
        public DateTime PaymentStartDate { get; set; }


        /// <summary>
        /// تعداد اقساط
        /// </summary>
        [Display(Name = "تعداد اقساط")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public byte NumberOfInstallments { get; set; } = 0;

        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        [Display(Name = "تاریخ ثبت")]
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// اقساط وام
        /// </summary>
        public ICollection<CharityLoanInstallments>? CharityLoanInstallments { get; set; }

        /// <summary>
        /// پرداخت شده است ؟
        /// </summary>
        [Display(Name = "پرداخت شده است ؟")]
        public bool IsDone { get; set; }

        /// <summary>
        /// حذف شده است ؟
        /// </summary>
        [Display(Name = "حذف شده است ؟")]
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public bool IsDelete { get; set; }
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    }
}
