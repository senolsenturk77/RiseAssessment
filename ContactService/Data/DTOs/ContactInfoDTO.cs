using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactService.Data.Entities
{
    public class ContactInfoDTO : BaseEntity
    {
        [Required]
        public ContactInfoType InfoType { get; set; }

        public string InfoTypeName { get { return InfoType.ToString(); } }

        [Required]
        public string Info { get; set; }

        [Required]
        public Guid ContactUuid { get; set; }

       

    }
}
