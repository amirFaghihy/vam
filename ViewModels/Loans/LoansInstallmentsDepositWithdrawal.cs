using System.ComponentModel.DataAnnotations;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.ViewModel
{
    /// <summary>
    /// وام، قسط، واریز، برداشت
    /// </summary>
    public class LoansInstallmentsDepositWithdrawal
    {
        /// <summary>
        /// شناسه حساب
        /// </summary>
        public int UserAccountId { get; set; }

        /// <summary>
        /// شناسه دریافت کننده وام
        /// </summary>
        public string LoanReceiverId { get; set; }

        /// <summary>
        /// کد
        /// </summary>
        [Display(Name = "کد")]
        public int RecordId { get; set; }

        /// <summary>
        /// نام و نام خانوادگی
        /// </summary>
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        /// <summary>
        /// نوع تراکنش
        /// </summary>
        [Display(Name = "نوع تراکنش")]
        public SummaryTransactionType TransactionType { get; set; }

        /// <summary>
        /// مبلغ
        /// </summary>
        [Display(Name = "مبلغ")]
        public double Price { get; set; }

        /// <summary>
        /// تاریخ تراکنش
        /// </summary>
        [Display(Name = "تاریخ تراکنش")]
        public DateTime? TransactionDateTime { get; set; }

        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        [Display(Name = "تاریخ ثبت")]
        public DateTime RegisterDate { get; set; }
    }
}
