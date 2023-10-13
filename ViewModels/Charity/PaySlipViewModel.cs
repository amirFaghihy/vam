using Aban.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban.ViewModels
{
    /// <summary>
    /// فیش حقوقی همراه با اضافات و کسورات و قسط وام
    /// </summary>
    public class PaySlipViewModel
    {
        /// <summary>
        /// شناسه ثبت کننده
        /// </summary>
        [Display(Name = "شناسه ثبت کننده")]
        public UserIdentity? UserRegistrar { get; set; }

        /// <summary>
        /// آی دی حقوق
        /// </summary>
        public int CharityWageId { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        /// <summary>
        /// نام
        /// </summary>
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        /// <summary>
        /// نام خانوادگی
        /// </summary>
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// نام پدر
        /// </summary>
        [Display(Name = "نام پدر")]
        public string? FatherName { get; set; } = null;

        /// <summary>
        /// کد ملی
        /// </summary>
        [Display(Name = "کد ملی")]
        public string? NationalCode { get; set; } = null;

        /// <summary>
        /// تاریخ تولد
        /// </summary>
        [Display(Name = "تاریخ تولد")]
        public DateTime? BirthDate { get; set; } = null;


        /// <summary>
        /// حقوق ثابت ماهانه
        /// </summary>
        [Display(Name = "حقوق ثابت ماهانه")]
        public float FixedSalary { get; set; } = 0;

        /// <summary>
        /// درصد
        /// </summary>
        [Display(Name = "درصد")]
        public byte PercentSalary { get; set; } = 0;

        /// <summary>
        /// جمع واریزیها
        /// </summary>
        [Display(Name = "جمع واریزیها")]
        public double TotalOfDeposits { get; set; }

        /// <summary>
        /// خالص دریافی
        /// نکته: خالص دریافتی را هرجا که لازم بود باید حساب کنید
        /// </summary>
        [Display(Name = "خالص دریافی")]
        public double NetSalaryReceived { get; set; }

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
        /// اضافات
        /// </summary>
        [Display(Name = "اضافات")]
        public List<CharityAddition>? CharityAdditions { get; set; }

        /// <summary>
        /// کسورات
        /// </summary>
        [Display(Name = "کسورات")]
        public List<CharityDeducation>? CharityDeducations { get; set; }

        /// <summary>
        /// اولین وام سررسید شده
        /// </summary>
        [Display(Name = "اولین وام سررسید شده")]
        public CharityLoan? CharityLoan { get; set; }


        /// <summary>
        /// اقساط سررسید شده
        /// </summary>
        [Display(Name = "اقساط سررسید شده")]
        public List<CharityLoanInstallments>? CharityLoanInstallments { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        /// <summary>
        /// شماره حساب/کارت
        /// </summary>
        [Display(Name = "شماره حساب/کارت")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string? AccountNumber { get; set; }
    }
}

