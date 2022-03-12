using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactService.Data.Entities
{
    public class ContactInfo:BaseEntity
    {
        public ContactInfoType InfoType { get; set; }
        public string Info { get; set; }


        public Guid ContactUuid { get; set; }

        [ForeignKey("ContactUuid")]
        public virtual Contact Contact { get; set; }

    }
}