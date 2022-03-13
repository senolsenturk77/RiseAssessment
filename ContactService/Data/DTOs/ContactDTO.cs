using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactService.Data.Entities
{
    public class ContactDTO :BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surename { get; set; }

        public string CompanyName { get; set; }

    }
}
