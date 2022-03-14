using ContactService.Data.Entities;
using System.Collections.Generic;

namespace ContactService.Data.Helper
{
    public static class HelperDTO
    {


        public static Contact ToContact(this ContactDTO contactDTO)
        {

            var resContact = new Contact();

            resContact.Uuid = contactDTO.Uuid;
            resContact.Name = contactDTO.Name;
            resContact.Surename = contactDTO.Surename;
            resContact.CompanyName = contactDTO.CompanyName;

            return resContact;

        }


        public static List<Contact> ToContact(this IEnumerable<ContactDTO> contacts)
        {

            var retList = new List<Contact>();

            foreach (var contact in contacts)
            {
                retList.Add(contact.ToContact());
            }

            return retList;

        }


        public static ContactDTO ToContactDTO(this Contact contact)
        {

            var resContact = new ContactDTO();

            resContact.Uuid = contact.Uuid;
            resContact.Name = contact.Name;
            resContact.Surename = contact.Surename;
            resContact.CompanyName = contact.CompanyName;

            return resContact;

        }

        public static List<ContactDTO> ToContactDTO(this IEnumerable<Contact> contacts)
        {

            var retList = new List<ContactDTO>();

            foreach (var contact in contacts)
            {
                retList.Add(contact.ToContactDTO());
            }

            return retList;

        }


        public static ContactInfo ToContactInfo(this ContactInfoDTO contactInfoDTO)
        {

            var resContactInfo = new ContactInfo();

            resContactInfo.Uuid = contactInfoDTO.Uuid;
            resContactInfo.InfoType = contactInfoDTO.InfoType;
            resContactInfo.Info = contactInfoDTO.Info;
            resContactInfo.ContactUuid = contactInfoDTO.ContactUuid;

            return resContactInfo;
        }




      
        public static ContactInfoDTO ToContactInfoDTO(this ContactInfo contactInfo)
        {

            var resContactInfo = new ContactInfoDTO();

            resContactInfo.Uuid = contactInfo.Uuid;
            resContactInfo.InfoType = contactInfo.InfoType;
            resContactInfo.Info = contactInfo.Info;
            resContactInfo.ContactUuid = contactInfo.ContactUuid;

            return resContactInfo;
        }


        public static List<ContactInfoDTO> ToContactInfoDTO(this IEnumerable<ContactInfo> contactInfos)
        {

            var retList = new List<ContactInfoDTO>();

            foreach (var contactInfo in contactInfos)
            {
                retList.Add(contactInfo.ToContactInfoDTO());
            }

            return retList;

        }


        public static ContactWithInfoDTO ToContactWithInfoDTO(this Contact contact)
        {

            var resContact = new ContactWithInfoDTO();

            resContact.Uuid = contact.Uuid;
            resContact.Name= contact.Name;
            resContact.Surename= contact.Surename;
            resContact.CompanyName= contact.CompanyName;

            foreach (var item in contact.ContactInformations)
            {

                resContact.ContactInformations.Add(item.ToContactInfoDTO());
            }


            return resContact;
        }
    }
}
