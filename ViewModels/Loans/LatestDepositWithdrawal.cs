using System.ComponentModel.DataAnnotations;

namespace Aban.ViewModel
{
    /// <summary>
    /// آخرین باقیمانده حساب
    /// </summary>
    public class LatestDepositWithdrawal
    {
        /// <summary>
        /// حساب
        /// </summary>
        [Display(Name = "حساب")]
        public int UserAccountId { get; set; }

        /// <summary>
        /// مانده بعد از تراکنش
        /// </summary>
        [Display(Name = "مانده بعد از تراکنش")]
        [Required(ErrorMessage = ("{0} را وارد کنید"))]
        public double TotalPriceAfterTransaction { get; set; }
    }
}
