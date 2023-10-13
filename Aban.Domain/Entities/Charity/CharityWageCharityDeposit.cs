using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban.Domain.Entities
{
    /// <summary>
    /// واریزیهای حقوق
    /// </summary>
    public class CharityWageCharityDeposit : BaseEntity<Guid>
    {
        [Display(Name ="شناسه فیش حقوقی")]
        public int CharityWageId { get; set; }

        [Display(Name ="فیش حقوقی")]
        [ForeignKey(nameof(CharityWageId))]
        public CharityWage? CharityWage { get; set; }


        [Display(Name ="شناسه پرداخت")]
        public int CharityDepositId { get; set; }

        [Display(Name ="شناسه پرداخت")]
        [ForeignKey(nameof(CharityDepositId))]
        public CharityDeposit? CharityDeposit { get; set; }
    }
}
