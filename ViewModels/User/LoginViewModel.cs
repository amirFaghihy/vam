using System.ComponentModel.DataAnnotations;

namespace Aban.ViewModels
{
    public class LoginViewModel : Aban.Domain.Entities.BaseEntity<int>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public string? Id { get; set; }

        [Display(Name = "UserName")]
        [Required(ErrorMessage = "لطفا نام کاربری خود را وارد نمایید")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required(ErrorMessage = "لطفا کلمه ی عبور خود را وارد نمایید")]
        public string Password { get; set; }

        [Required(ErrorMessage = "این فیلد اجباری است")]
        [Display(Name = "تلفن همراه")]
        //[RegularExpression(@"^\d*", ErrorMessage = "شماره تلفن همراه وارد شده صحیح نمی باشد")]
        [MaxLength(11, ErrorMessage = "شماره تلفن همراه وارد شده باید 11 رقم باشد")]
        [MinLength(11, ErrorMessage = "شماره تلفن همراه وارد شده باید 11 رقم باشد")]
        [RegularExpression(@"^((09)|(۰۹)|(۰9)|(0۹))(\d|۱|۲|۳|۴|۵|۶|۷|۸|۹|۰){9}", ErrorMessage = "لطفاً فقط عدد و اعداد را به صورت صحیح وارد کنید وارد کنید. مثال: 09123456789")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "این فیلد اجباری است")]
        //[RegularExpression(@"^\d*", ErrorMessage = "کد به صورت عددی باید وارد شود")]
        [Display(Name = "کد پیامک شده")]
        [RegularExpression(@"^(\d|۱|۲|۳|۴|۵|۶|۷|۸|۹|۰){4}", ErrorMessage = "لطفاً فقط عدد و اعداد را به صورت صحیح وارد کنید وارد کنید.")]
        public string SMSCode { get; set; }


        [Display(Name = "Remember me?")]
        public bool? RememberMe { get; set; } = null;

        //[RegularExpression(@"^\d*", ErrorMessage = "حاصل جمع را می بایست به صورت عدد بنویسید")]
        //[Required(ErrorMessage = "این فیلد اجباری است")]
        //[Remote("ValidateCheckSpam", "Validate", ErrorMessage = "حاصل جمع اشتباه است")]
        //public int Sum { get; set; }
    }

}
