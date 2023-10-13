using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Aban.Domain.Entities
{
    public class BlogCategory : BaseEntity<int>
    {
        #region ForeignKeys

        /// <summary>
        /// شناسه ثبت کننده
        /// </summary>
        [Display(Name = "شناسه ثبت کننده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string UserIdentityId { get; set; }

        /// <summary>
        /// شناسه ثبت کننده
        /// </summary>
        [ForeignKey("UserIdentityId")]
        [Display(Name = "شناسه ثبت کننده")]
        public UserIdentity? UserIdentity { get; set; }

        /// <summary>
        /// شناسه پدر
        /// </summary>
        [Display(Name = "شناسه پدر")]
        public int? ParentId { get; set; }
        /// <summary>
        /// شناسه پدر
        /// </summary>
        [Display(Name = "شناسه پدر")]
        [ForeignKey("ParentId")]
        public BlogCategory? Parent { get; set; }

        #endregion

        /// <summary>
        /// عنوان
        /// </summary>
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "عنوان نمی‌تواند بیشتر از 50 کاراکتر باشد.")]
        public string Title { get; set; }

        /// <summary>
        /// نمایش؟
        /// </summary>
        [Display(Name = "نمایش؟")]
        public bool IsVisible { get; set; }

        /// <summary>
        /// آدرس
        /// </summary>
        [Display(Name = "آدرس")]
        [MaxLength(100, ErrorMessage = "آدرس مطلب نمی تواند بیشتر از 100 کاراکتر باشد.")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string BlogCategoryURL { get; set; }
    }
}
