using System.ComponentModel.DataAnnotations;

namespace Aban.Domain.Entities
{
    public class State : BaseEntity<int>
    {


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [MaxLength(50, ErrorMessage = "نام نمی تواند بیشتر از 50 کاراکتر باشد.")]
        [Required(ErrorMessage = "لطفا نام استان را وارد کنید.")]
        public string Title { get; set; }
    }
}
