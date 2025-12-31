using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aban.Domain.Entities
{
    public class City : BaseEntity<int>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public int StateId { get; set; }

        [ForeignKey("StateId")]
        public State State { get; set; }

        /// <summary>
        /// نام شهر
        /// </summary>
        [MaxLength(50, ErrorMessage = "نام شهر نمی‌تواند بیشتر از 50 کاراکتر باشد.")]
        [Required(ErrorMessage = "لطفا نام شهر را وارد کنید.")]
        public string Title { get; set; }
    }
}
