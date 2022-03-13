
using System;
using System.ComponentModel.DataAnnotations;

namespace ReportService.Data.Entities
{
    public abstract class BaseEntity : IEntity
    {
        [Key]
        [Required]
        public Guid Uuid { get; set; }
    }
}
