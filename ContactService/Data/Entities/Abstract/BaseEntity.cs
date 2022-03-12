
using System;
using System.ComponentModel.DataAnnotations;

namespace ContactService.Data.Entities
{
    public abstract class BaseEntity : IEntity
    {
        [Key]
        [Required]
        public Guid Uuid { get; set; }
    }
}
