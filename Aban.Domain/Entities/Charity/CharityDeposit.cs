using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Aban.Domain.Entities
{
    /// <summary>
    /// واریزی ها
    /// </summary>
    public class CharityDeposit : BaseEntity<int>
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
        public UserIdentity UserIdentity { get; set; }


        /// <summary>
        /// مددکار/خیّر
        /// </summary>
        [Display(Name = "مددکار/خیّر")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Required(ErrorMessage = "{0} را انتخاب کنید")]
        public string HelperId { get; set; }

        /// <summary>
        /// مددکار/خیّر
        /// </summary>
        [ForeignKey("HelperId")]
        [Display(Name = "مددکار/خیّر")]
        public UserIdentity Helper { get; set; }



        /// <summary>
        /// شماره حساب/کارت
        /// </summary>
        [Display(Name = "شماره حساب/کارت")]
        [Required(ErrorMessage = "{0} را انتخاب کنید")]
        public int CharityAccountId { get; set; }
        /// <summary>
        /// شماره حساب/کارت
        /// </summary>
        [Display(Name = "شماره حساب/کارت")]
        [Required(ErrorMessage = "{0} را انتخاب کنید")]
        public CharityAccount CharityAccount { get; set; }

        #endregion


        /// <summary>
        /// مقدار
        /// </summary>
        [Display(Name = "مقدار")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        [RegularExpression(@"^\d(.*\d)?$", ErrorMessage = "{0} فقط میتواند عدد باشد")]
        public double Amount { get; set; }

        /// <summary>
        /// تأیید ؟
        /// </summary>
        [Display(Name = "تأیید ؟")]
        public bool IsConfirm { get; set; }

        /// <summary>
        /// آیا سود این واریزی حساب شده است یا خیر ؟
        /// </summary>
        [Display(Name = "سود محاسبه شده است ؟")]
        public bool IsDone { get; set; } = false;

        /// <summary>
        /// شماره پیگیری
        /// </summary>
        [Display(Name = "شماره سند/شماره سریال/شماره پیگیری")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string IssueTracking { get; set; }


        /// <summary>
        /// شرح سند
        /// </summary>
        [Display(Name = "شرح سند")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Description { get; set; }

        /// <summary>
        /// تاریخ سند
        /// </summary>
        [Display(Name = "تاریخ واریزی")]
        public DateTime DocumentRegisterDateTime { get; set; }


        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        [Display(Name = "تاریخ ثبت")]
        public DateTime RegisterDate { get; set; }


        /// <summary>
        /// تاریخ تأیید
        /// </summary>
        [Display(Name = "تاریخ تأیید")]
        public DateTime? ConfirmDate { get; set; } = null;


        //TODO:باید یک فیلد اضاف شود برای  دسترسی به رکورد بانکی
        public int? CharityBankRecordId { get; set; }
        [ForeignKey(nameof(CharityBankRecordId))]
        public CharityBankRecord? CharityBankRecord { get; set; }

        /// <summary>
        /// چهار رقم آخر کارت
        /// </summary>
        /// 
        [Display(Name = "چهار رقم آخر کارت")]
        [RegularExpression("^[0-9]{4}$", ErrorMessage = "{0} فقط میتواند شامل چهار رقم باشد.")]
        public string? LastFourDigits { get; set; }

        [Display(Name = "شیوه ی  تایید")]
        public ConfirmType ConfirmType { get; set; } = ConfirmType.None;

    }

    public enum ConfirmType : byte
    {
        [Description("تایید نشده")]
        None = 0,
        [Description("بر اساس کد پیگیری")]
        By_Issue_number = 1,
        [Description("بر اساس شماره کارت")]
        By_Card_Number = 2,
        [Description("توسط ادمین")]
        By_Admin = 3
    }
}
