using Aban.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban.ViewModels
{
    public class UserIdentityViewModel
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        //[Key]
        //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        /// <summary>
        /// نام کاربری 
        /// </summary>
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "نام کاربری را وارد کنید")]
        public string UserName { get; set; }


        /// <summary>
        /// شماره تلفن همراه
        /// </summary>
        [Display(Name = "شماره تلفن همراه")]
        [Required(ErrorMessage = "لطفا شماره تلفن همراه را وارد کنید")]
        [MaxLength(11, ErrorMessage = "شماره تلفن همراه وارد شده حداکثر باید 11 باشد")]
        [RegularExpression(@"^((09)|(۰۹)|(۰9)|(0۹))(\d|۱|۲|۳|۴|۵|۶|۷|۸|۹|۰){9}", ErrorMessage = "لطفاً فقط عدد و اعداد را به صورت صحیح وارد کنید. مثال: 09123456789")]
        public string PhoneNumber { get; set; }

        /// <summary>
		/// نام
		/// </summary>
		[Display(Name = "نام")]
        [Required(ErrorMessage = "نام را وارد نمایید")]
        [MaxLength(20, ErrorMessage = "نام حداکثر میتواند 20 کاراکتر باشد.")]
        public string FirstName { get; set; }

        /// <summary>
        /// نام خانوادگی
        /// </summary>
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "نام خانوادگی را وارد نمایید")]
        [MaxLength(30, ErrorMessage = "نام خانوادگی حداکثر میتواند 30 کاراکتر باشد.")]
        public string LastName { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// کد ملی
        /// </summary>
        [Display(Name = "کد ملی")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "کد ملی فقط می تواند عدد باشد.")]
        [MaxLength(10, ErrorMessage = "کد ملی نمی تواند بیشتر از 10 کاراکتر باشد.")]
        public string? NationalCode { get; set; }

        /// <summary>
        /// ایمیل
        /// </summary>
        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "ایمیل معتبر را وارد نمایید")]
        //[Remote("ValidateEmail", "UserIdentity", "CMS", ErrorMessage = "این ایمیل موجود است", AdditionalFields = "Id")]
        [RegularExpression(@"^[_A-Za-z0-9-\+]+(\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*(\.[A-Za-z]{2,4})$", ErrorMessage = "ایمیل را بدرستی وارد کنید")]
        public string? Email { get; set; }

        /// <summary>
        /// آقا؟
        /// </summary>
        [Display(Name = "آقا؟")]
        public bool IsMale { get; set; }


        /// <summary>
        /// رمز عبور
        /// </summary>
        [Display(Name = "رمز عبور")]
        public string? Password { get; set; }


        /// <summary>
        /// مسدود شده؟
        /// </summary>
        [Display(Name = "مسدود شده؟")]
        [DefaultValue(false)]
        public bool IsLocked { get; set; }

        /// <summary>
        /// تایید شده؟
        /// </summary>
        [Display(Name = "تایید شده؟")]
        [DefaultValue(false)]
        public bool IsConfirm { get; set; }

        /// <summary>
        /// تاریخ تولد
        /// </summary>
        [Display(Name = "تاریخ تولد")]
        public DateTime? BirthDate { get; set; } = null;

        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        [Display(Name = "تاریخ ثبت")]
        public DateTime? RegisterDate { get; set; } = null;

        /// <summary>
        /// لیست رولها
        /// </summary>
        [Display(Name = "نقشهای کاربر")]
        [Required(ErrorMessage = "حداقل یک نقش برای کاربر انتخاب کنید")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public IEnumerable<string> RoleNames { get; set; }
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
        /// ثبت کننده
        /// </summary>
        [Display(Name = "ثبت کننده")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string UserRegistrarId { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// شناسه ثبت کننده
        /// </summary>
        [ForeignKey("UserRegistrarId")]
        [Display(Name = "شناسه ثبت کننده")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public UserIdentity UserRegistrar { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


        /// <summary>
        /// نام پدر
        /// </summary>
        [Display(Name = "نام پدر")]
        [MaxLength(100, ErrorMessage = "نام پدر نمی تواند بیشتر از {0} کاراکتر باشد")]
        public string? FatherName { get; set; } = null;


        /// <summary>
        /// تاریخ شروع فعالیت
        /// </summary>
        [Display(Name = "تاریخ شروع فعالیت")]
        public DateTime? StartActivityDateTime { get; set; }

        /// <summary>
        /// شماره کارت/حساب
        /// </summary>
        [Display(Name = "شماره کارت/حساب")]
        public string? CardNumber { get; set; }
    }
}
