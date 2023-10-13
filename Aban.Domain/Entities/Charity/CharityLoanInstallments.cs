using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Aban.Domain.Entities
{
    /// <summary>
    /// اقساط وام
    /// </summary>
    public class CharityLoanInstallments : BaseEntity<int>
    {
        #region ForeignKeys

        /// <summary>
        /// شناسه وام
        /// </summary>
        [Display(Name = "شناسه وام")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Required(ErrorMessage = "{0} را انتخاب کنید")]
        public int CharityLoanId { get; set; }

        /// <summary>
        /// شناسه وام
        /// </summary>
        [ForeignKey("CharityLoanId")]
        [Display(Name = "شناسه وام")]
        public CharityLoan? CharityLoan { get; set; }


        #endregion


        /// <summary>
        /// مبلغ قسط
        /// </summary>
        [Display(Name = "مبلغ قسط")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        [RegularExpression(@"^\d(.*\d)?$", ErrorMessage = "{0} فقط میتواند عدد باشد")]
        public double InstallmentAmount { get; set; } = 0;


        /// <summary>
        /// موعد پرداخت
        /// </summary>
        [Display(Name = "موعد پرداخت")]
        public DateTime PaymentDue { get; set; }


        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        [Display(Name = "تاریخ ثبت")]
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// تاریخ پرداخت
        /// </summary>
        [Display(Name = "تاریخ پرداخت")]
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// پرداخت شده است ؟
        /// </summary>
        [Display(Name = "پرداخت شده است ؟")]
        public bool IsDone { get; set; }

        /// <summary>
        /// حذف شده است ؟
        /// </summary>
        [Display(Name = "حذف شده است ؟")]
#pragma warning disable CS0108
        public bool IsDelete { get; set; }
#pragma warning restore CS0108
    }
}
