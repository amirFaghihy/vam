using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aban.Domain.Entities
{
    /// <summary>
    /// وبلاگ و مقالات
    /// </summary>
    public class BlogArticle : BaseEntity<Guid>
    {
        #region ForeignKeys

        /// <summary>
        /// شناسه ثبت کننده
        /// </summary>
        [Display(Name = "شناسه ثبت کننده")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string UserIdentityId { get; set; }

        /// <summary>
        /// شناسه ثبت کننده
        /// </summary>
        [ForeignKey("UserIdentityId")]
        [Display(Name = "شناسه ثبت کننده")]
        public UserIdentity? UserIdentity { get; set; }

        /// <summary>
        /// شناسه نویسنده
        /// </summary>
        [Display(Name = "شناسه نویسنده")]
        [Required(ErrorMessage = "{0} را انتخاب کنید")]
        public string WriterId { get; set; }

        /// <summary>
        /// شناسه نویسنده
        /// </summary>
        [ForeignKey("WriterId")]
        [Display(Name = "شناسه نویسنده")]
        public UserIdentity? Writer { get; set; }

        #endregion

        /// <summary>
        /// عنوان
        /// </summary>
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// چکیده
        /// </summary>
        [Display(Name = "چکیده")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Intro { get; set; }

        /// <summary>
        /// متن کامل
        /// </summary>
        [Display(Name = "متن کامل")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Description { get; set; }

        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        [Display(Name = "تاریخ ثبت")]
        public DateTime RegisterDate { get; set; }


        /// <summary>
        /// آدرس
        /// </summary>
        [Display(Name = "آدرس")]
        public string? BlogArticleURL { get; set; }

        /// <summary>
        /// فعال ؟
        /// </summary>
        [Display(Name = "فعال ؟")]
        public bool Isvisible { get; set; }

       

        /// <summary>
        /// عکس مطلب
        /// </summary>
        [Display(Name = "عکس مطلب")]
        public string? ImageName { get; set; }


        /// <summary>
        /// ویدئوی مطلب
        /// </summary>
        [Display(Name = "ویدیوی مطلب")]
        public string? VideoName { get; set; }


        /// <summary>
        /// لینک ویدئوی مطلب
        /// </summary>
        [Display(Name = "لینک ویدیوی مطلب")]
        public string? Videolink { get; set; }
    }
}
