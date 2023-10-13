using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Aban.Domain.Entities
{
    /// <summary>
    /// حقوق
    /// </summary>
    public class CharityWage : BaseEntity<int>
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
        /// حقوق گیرنده
        /// </summary>
        [Display(Name = "حقوق گیرنده")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Required(ErrorMessage = "{0} را انتخاب کنید")]
        public string WageReceiverId { get; set; }

        /// <summary>
        /// حقوق گیرنده
        /// </summary>
        [ForeignKey("WageReceiverId")]
        [Display(Name = "حقوق گیرنده")]
        public UserIdentity? WageReceiver { get; set; }


        #endregion


        /// <summary>
        /// شماره حساب/کارت
        /// </summary>
        [Display(Name = "شماره حساب/کارت")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string AccountNumber { get; set; }


        /// <summary>
        /// شرح سند
        /// </summary>
        [Display(Name = "شرح سند")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Description { get; set; }


        /// <summary>
        /// حقوق از تاریخ
        /// </summary>
        [Display(Name = "حقوق از تاریخ")]
        public DateTime WageDateFrom { get; set; }

        /// <summary>
        /// حقوق تا تاریخ
        /// </summary>
        [Display(Name = "حقوق تا تاریخ")]
        public DateTime WageDateTo { get; set; }


        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        [Display(Name = "تاریخ ثبت")]
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// اضافات فیش حقوقی
        /// </summary>
        public ICollection<CharityWageCharityAddition>? CharityWageCharityAdditions { get; set; }

        /// <summary>
        /// کسورات از فیش حقوقی
        /// </summary>
        public ICollection<CharityWageCharityDeduction>? CharityWageCharityDeductions { get; set; }

        /// <summary>
        /// واریزیهای تأیید شده ی حقوق
        /// </summary>
        public ICollection<CharityWageCharityDeposit>? CharityWageCharityDeposit { get; set; }


        /// <summary>
        /// تاریخ ویرایش
        /// </summary>
        [Display(Name = "تاریخ ویرایش")]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// حقوق ثابت ماهانه
        /// </summary>
        [Display(Name = "حقوق ثابت ماهانه")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        [RegularExpression(@"^\d(.*\d)?$", ErrorMessage = "{0} فقط میتواند عدد باشد")]
        public float FixedSalary { get; set; } = 0;


        /// <summary>
        /// درصد
        /// </summary>
        [Display(Name = "درصد")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public byte PercentSalary { get; set; } = 0;


        /// <summary>
        /// جمع واریزیها
        /// </summary>
        [Display(Name = "جمع واریزیها")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public double SumOfDeposits { get; set; } = 0;


        /// <summary>
        /// آیا سودش برای سرکاربر محاسبه شده است ؟
        /// </summary>
        [Display(Name = "آیا سودش برای سرکاربر محاسبه شده است ؟")]
        public bool IsUsedForForeman { get; set; } = false;
    }
}
