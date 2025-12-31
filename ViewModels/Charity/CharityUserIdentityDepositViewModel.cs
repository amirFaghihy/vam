using Aban.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Aban.ViewModels
{
    public class CharityUserIdentityDepositViewModel
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Display(Name = "شناسه کاربری")]
        public string UserIdentityId { get; set; }

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
        /// نام پدر
        /// </summary>
        [Display(Name = "نام پدر")]
        public string? FatherName { get; set; } = null;

        /// <summary>
        /// جمع واریزیها
        /// </summary>
        [Display(Name = "جمع واریزیها")]
        public double TotalOfDeposits { get; set; }

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
        /// اولین قسط سررسید شده
        /// </summary>
        [Display(Name = "اولین قسط سررسید شده")]
        public List<CharityLoanInstallments>? CharityLoanInstallments { get; set; }


        /// <summary>
        /// آی دی رول
        /// </summary>
        [Display(Name = "آی دی رول")]
        public string? RoleId { get; set; }

    }
}

