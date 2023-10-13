using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban.Domain.Entities
{
    /// <summary>
    /// کسورات فیش حقوقی
    /// </summary>
    public class CharityWageCharityDeduction : BaseEntity<int>
    {
        [Display(Name ="شناسه فیش حقوقی")]
        public int CharityWageId { get; set; }

        [Display(Name ="فیش حقوقی")]
        [ForeignKey(nameof(CharityWageId))]
        public CharityWage? CharityWage { get; set; }


        [Display(Name ="شناسه کسر از فیش")]
        public int CharityDeducationId { get; set; }

        [Display(Name ="کسر از فیش")]
        [ForeignKey(nameof(CharityDeducationId))]
        public CharityDeducation? CharityDeducation { get; set; }
    }
}
