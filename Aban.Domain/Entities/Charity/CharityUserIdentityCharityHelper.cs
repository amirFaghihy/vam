using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban.Domain.Entities
{
    /// <summary>
    /// خیر وابسطه به منشی
    /// و
    /// منشی وابسه به سرکاربر
    /// </summary>
    public class CharityUserIdentityCharityHelper : BaseEntity<int>
    {

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
        /// مددکار/خیّر/منشی
        /// </summary>
        [Display(Name = "مددکار/خیّر/منشی")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string HelperId { get; set; }

        /// <summary>
        /// مددکار/خیّر/منشی
        /// </summary>
        [ForeignKey("HelperId")]
        [Display(Name = "مددکار/خیّر/منشی")]
        public UserIdentity Helper { get; set; }


       

        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        [Display(Name ="تاریخ ثبت")]
        public DateTime RegisterDate { get; set; }
    }
}
