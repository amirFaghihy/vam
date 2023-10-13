using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban.Domain.Entities
{
    /// <summary>
    /// اضافات فیش حقوقی
    /// </summary>
    public class CharityWageCharityAddition : BaseEntity<int>
    {
        [Display(Name = "شناسه فیش حقوقی")]
        public int CharityWageId { get; set; }

        [Display(Name = " فیش حقوقی")]
        [ForeignKey(nameof(CharityWageId))]
        public CharityWage? CharityWage { get; set; }


        [Display(Name = "شناسه اضافات فیش")]
        public int CharityAdditionId { get; set; }

        [Display(Name = "اضافات فیش")]
        [ForeignKey(nameof(CharityAdditionId))]
        public CharityAddition? CharityAddition { get; set; }


    }
}
