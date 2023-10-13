using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Aban.Domain.Entities
{
    /// <summary>
    /// رکورهای بانک
    /// </summary>
    public class CharityBankRecord : BaseEntity<int>
    {
        #region ForeignKeys

        /// <summary>
        /// ثبت کننده
        /// </summary>
        //[Display(Name = "ثبت کننده")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        //public string? UserIdentityId { get; set; } = string.Empty;

        /// <summary>
        /// ثبت کننده
        /// </summary>
        //[ForeignKey("UserIdentityId")]
        //[Display(Name = "ثبت کننده")]
        //public UserIdentity? UserIdentity { get; set; } = null;

        #endregion

        /// <summary>
        /// شماره برگه/واریز کننده
        /// </summary>
        [Display(Name = "شماره برگه/واریز کننده")]
        public string PaperNumber { get; set; }

        /// <summary>
        /// شماره سند/شماره سریال/شماره پیگیری
        /// </summary>
        [Display(Name = "شماره سند/شماره سریال/شماره پیگیری")]
        public string DocumentNumber { get; set; }


        /// <summary>
        /// شعبه
        /// </summary>
        [Display(Name = "شعبه")]
        public string? Branch { get; set; } = null;

        /// <summary>
        /// کد حسابگری
        /// </summary>
        [Display(Name = "کد حسابگری/کارت واریز کننده")]
        public string? Accountant { get; set; } = null;


        /// <summary>
        /// شناسه واریز
        /// </summary>
        [Display(Name = "شناسه واریز بانکی")]
        public string? BankDepositId { get; set; } = null;


        /// <summary>
        /// بستانکار/واریزی
        /// </summary>
        [Display(Name = "بستانکار/واریزی")]
        public double Creditor { get; set; }

        /// <summary>
        /// بدهکار/برداشت
        /// </summary>
        [Display(Name = "بدهکار/برداشت")]
        public double Debtor { get; set; }

        /// <summary>
        /// موجودی
        /// </summary>
        [Display(Name = "موجودی")]
        public double Inventory { get; set; }

        /// <summary>
        /// شرح سند
        /// </summary>
        [Display(Name = "شرح سند")]
        public string Description { get; set; }

        /// <summary>
        /// تاریخ سند
        /// </summary>
        [Display(Name = "تاریخ سند")]
        public DateTime DocumentRegisterDateTime { get; set; }


        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        [Display(Name = "تاریخ ثبت")]
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// شناسه حساب
        /// </summary>
        [Display(Name = "حساب بانکی")]
        public int CharityAccountId { get; set; }

        /// <summary>
        /// شناسه حساب
        /// </summary>
        [Display(Name = "حساب بانکی")]
        [ForeignKey(nameof(CharityAccountId))]
        public CharityAccount CharityAccount { get; set; }

        /// <summary>
        /// در حال استفاده
        /// </summary>
        [Display(Name = "در حال استفاده")]
        public bool IsInUse { get; set; }= false;
    }
}
