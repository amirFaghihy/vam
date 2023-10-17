using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Aban.Domain.Entities
{
    public class UserIdentity : IdentityUser
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        //[Key]
        //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        //public override string Id { get; set; }

        /// <summary>
        /// نام کاربری 
        /// </summary>
        //[Display(Name = "نام کاربری")]
        //[Required(ErrorMessage = "نام کاربری را وارد کنید")]
        //public override string UserName { get; set; }

        // برای آی دی و یوزرنیم و شماره موبایل بیشتر از دو هفته دهنم سرویس شد
        // آخرش متوجه شدم که این سه تا فیلد رو نباید اُوررایت کنم که بتونم
        // توی کدها هندلشون کنم
        // مایکروسافت خر است 🤣🤣🤣

        /// <summary>
        /// شماره تلفن همراه
        /// </summary>
        //[Display(Name = "شماره تلفن همراه")]
        //[Required(ErrorMessage = "لطفا شماره تلفن همراه را وارد کنید")]
        //[MaxLength(11, ErrorMessage = "شماره تلفن همراه وارد شده حداکثر باید 11 باشد")]
        //[RegularExpression(@"^((09)|(۰۹)|(۰9)|(0۹))(\d|۱|۲|۳|۴|۵|۶|۷|۸|۹|۰){9}", ErrorMessage = "لطفاً فقط عدد و اعداد را به صورت صحیح وارد کنید. مثال: 09123456789")]
        //public override string PhoneNumber { get; set; }

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

        /// <summary>
        /// کد ملی
        /// </summary>
        [Display(Name = "کد ملی")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "کد ملی فقط می تواند عدد باشد.")]
        [MaxLength(10, ErrorMessage = "کد ملی نمی تواند بیشتر از 10 کاراکتر باشد.")]
        public string NationalCode { get; set; }

        /// <summary>
        /// ایمیل
        /// </summary>
        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "ایمیل معتبر را وارد نمایید")]
        //[Remote("ValidateEmail", "UserIdentity", "CMS", ErrorMessage = "این ایمیل موجود است", AdditionalFields = "Id")]
        [RegularExpression(@"^[_A-Za-z0-9-\+]+(\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*(\.[A-Za-z]{2,4})$", ErrorMessage = "ایمیل را بدرستی وارد کنید")]
        public override string Email { get; set; }


        /// <summary>
        /// آقا؟
        /// </summary>
        [Display(Name = "آقا؟")]
        public bool IsMale { get; set; }

        /// <summary>
        /// عکس پروفایل
        /// </summary>
        [Display(Name = "عکس پروفایل")]
        public string? ProfileImage { get; set; }

        /// <summary>
        /// رمز عبور
        /// </summary>
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }


        /// <summary>
        /// شهر
        /// </summary>
        [Display(Name = "شهر")]
        public int? CityId { get; set; }
        /// <summary>
        /// شهر
        /// </summary>
        [Display(Name = "شهر")]
        [ForeignKey("CityId")]
        public City City { get; set; }

        /// <summary>
        /// آدرس محل سکونت
        /// </summary>
        [Display(Name = "آدرس محل سکونت")]
        public string? HomeAddress { get; set; }

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
        public bool IsConfirm { get; set; } = true;

        /// <summary>
        /// نام پدر
        /// </summary>
        [Display(Name = "نام پدر")]
        [MaxLength(100, ErrorMessage = "نام پدر نمی تواند بیشتر از {0} کاراکتر باشد")]
        public string? FatherName { get; set; } = null;

        /// <summary>
        /// تاریخ تولد
        /// </summary>
        [Display(Name = "تاریخ تولد")]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        [Display(Name = "تاریخ ثبت")]
        public DateTime RegisterDate { get; set; }


        /// <summary>
        /// تاریخ ویرایش
        /// </summary>
        [Display(Name = "تاریخ ویرایش")]
        public DateTime? ModifiedDate { get; set; }

    }
}
