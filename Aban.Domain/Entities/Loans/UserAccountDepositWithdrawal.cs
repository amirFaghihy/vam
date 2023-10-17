using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Domain.Entities
{
    /// <summary>
    /// واریز و برداشت از حساب
    /// </summary>
    public class UserAccountDepositWithdrawal : BaseEntity<int>
    {
        #region ForeignKeys

        [Display(Name = "حساب")]
        public int UserAccountId { get; set; }

        [Display(Name = "حساب")]
        [ForeignKey(nameof(UserAccountId))]
        public UserAccount? UserAccount { get; set; }

        #endregion


        /// <summary>
        /// مبلغ واریز/برداشت
        /// </summary>
        [Display(Name = "مبلغ واریز/برداشت")]
        [Required(ErrorMessage = ("{0} را وارد کنید"))]
        public double Price { get; set; } = 0;

        /// <summary>
        /// مانده بعد از تراکنش
        /// </summary>
        [Display(Name = "مانده بعد از تراکنش")]
        [Required(ErrorMessage = ("{0} را وارد کنید"))]
        public double TotalPriceAfterTransaction { get; set; }

        /// <summary>
        /// واریز/برداشت
        /// </summary>
        [Display(Name = "واریز/برداشت")]
        public TransactionType AccountTransactionType { get; set; }

        /// <summary>
        /// روش واریز/برداشت
        /// </summary>
        [Display(Name = "روش واریز/برداشت")]
        public TransactionMethod AccountTransactionMethod { get; set; }

        /// <summary>
        /// تاریخ تراکنش
        /// </summary>
        [Display(Name = "تاریخ تراکنش")]
        [Required(ErrorMessage = ("{0} را وارد کنید"))]
        public DateTime TransactionDateTime { get; set; }


        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        [Display(Name = "تاریخ ثبت")]
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        [Display(Name = "توضیحات")]
        public string? Description { get; set; }
    }
}
