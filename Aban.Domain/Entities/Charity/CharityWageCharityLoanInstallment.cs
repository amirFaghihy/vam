using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban.Domain.Entities
{
    /// <summary>
    /// اقساطی که برای حقوق کم شده است
    /// </summary>
    public class CharityWageCharityLoanInstallment : BaseEntity<int>
    {
        [Display(Name = "شناسه فیش حقوقی")]
        public int CharityWageId { get; set; }

        [Display(Name = " فیش حقوقی")]
        [ForeignKey(nameof(CharityWageId))]
        public CharityWage? CharityWage { get; set; }


        [Display(Name = "شناسه قسط")]
        public int CharityLoanInstallmentId { get; set; }

        [Display(Name = "شناسه قسط")]
        [ForeignKey(nameof(CharityLoanInstallmentId))]
        public CharityLoanInstallments? CharityLoanInstallments { get; set; }


    }
}
