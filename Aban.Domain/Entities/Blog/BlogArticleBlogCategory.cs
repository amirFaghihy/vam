using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Aban.Domain.Entities
{
    public class BlogArticleBlogCategory : BaseEntity<Guid>
    {
        #region ForeignKeys

        /// <summary>
        /// شناسه دسته بندی
        /// </summary>
        [Display(Name = "شناسه دسته بندی")]
        public int BlogCategoryId { get; set; }

        /// <summary>
        /// شناسه دسته بندی
        /// </summary>
        [Display(Name = "شناسه دسته بندی")]
        [ForeignKey("BlogCategoryId")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public BlogCategory BlogCategory { get; set; }

        /// <summary>
        /// شناسه مطلب
        /// </summary>
        [Display(Name = "شناسه مطلب")]
        public Guid BlogArticleId { get; set; }

        /// <summary>
        /// شناسه مطلب
        /// </summary>
        [Display(Name = "شناسه مطلب")]
        [ForeignKey("BlogArticleId")]
        public BlogArticle BlogArticle { get; set; }

        #endregion
    }
}
