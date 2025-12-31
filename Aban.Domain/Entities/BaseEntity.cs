using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban.Domain.Entities
{
    public interface IBaseEntity<TKey> where TKey : IEquatable<TKey>
    {
    }

    public class BaseEntity<TKey> : IBaseEntity<TKey> where TKey : IEquatable<TKey>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public TKey Id { get; set; }

        /// <summary>
        /// حذف شده ؟
        /// </summary>
        [Display(Name = "حذف شده ؟")]
        public bool IsDelete { get; set; } = false;

        [NotMapped]
        public ResultStatusOperation ResultStatusOperation { get; set; }
    }
}
