using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DumpTruckManagementSystem.Domain.Common.Base
{
    public class EntityBase<T>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string CreatedBy { get; set; } = null!;

        public DateTime? EditAt { get; set; }

        public string? EditBy { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
