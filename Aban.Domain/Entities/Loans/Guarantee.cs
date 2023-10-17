using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Aban.Domain.Enumerations.Enumeration;
using Microsoft.AspNetCore.Authentication;

namespace Aban.Domain.Entities
{
    /// <summary>
    /// ضمانت وام
    /// </summary>
    public class Guarantee : BaseEntity<int>
    {
        #region ForeignKeys

        /// <summary>
        /// ضامن
        /// </summary>
        [Display(Name = "ضامن")]
#pragma warning disable CS8618
        public string GuaranteeId { get; set; }

        /// <summary>
        /// ضامن
        /// </summary>
        [ForeignKey("GuaranteeUserId")]
        [Display(Name = "ضامن")]
        public UserIdentity? GuaranteeUser { get; set; }

        #endregion

        /// <summary>
        /// شماره چک
        /// </summary>
        [Display(Name = "شماره چک")]
        public string? ChequeNumber { get; set; }

        /// <summary>
        /// نام بانک
        /// </summary>
        [Display(Name ="نام بانک")]
        public BankName? BankName { get; set; }

        /// <summary>
        /// مبلغ چک ضمانت
        /// </summary>
        [Display(Name ="مبلغ چک ضمانت")]
        public double? ChequePrice { get; set; }

        /// <summary>
        /// شماره سفته
        /// </summary>
        [Display(Name ="شماره سفته")]
        public string? BankDraftNumber { get; set; }
        
        /// <summary>
        /// مبلغ سفته
        /// </summary>
        [Display(Name ="مبلغ سفته")]
        public double? BankDraftPrice { get; set; }

        /// <summary>
        /// وزن و مشخصات طلای ضمانتی
        /// </summary>
        [Display(Name = "وزن و مشخصات طلای ضمانتی")]
        public string? GoldGuarantee { get; set; }

        /// <summary>
        /// مشخصات فیش حقوقی ضمانتی
        /// </summary>
        [Display(Name = "مشخصات فیش حقوقی ضمانتی")]
        public string? PaySlip { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        [Display(Name = "تاریخ ثبت")]
        public DateTime RegisterDate { get; set; }
    }

}
