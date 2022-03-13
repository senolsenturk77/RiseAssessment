
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactService.Data.Entities
{
    public class ContactWithInfoDTO : BaseEntity
    {
        public string Name { get; set; }

        public string Surename { get; set; }

        public string CompanyName { get; set; }

        public List<ContactInfoDTO> ContactInformations { get; set; } = new List<ContactInfoDTO>();

    }
}