using ContactService.Data.Entities;
using ContactService.Data.Helper;
using ContactService.Data.Repositories;
using ContactService.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactInfoController : ControllerBase
    {

        private readonly ILogger<ContactInfoController> _logger;
        private readonly IContactRepository _contactRepository;
        private readonly IContactInfoRepository _contactInfoRepository;

        public ContactInfoController(ILogger<ContactInfoController> logger, IContactRepository contactRepository, IContactInfoRepository contactInfoRepository)
        {

            _logger = logger;
            _contactRepository = contactRepository;
            _contactInfoRepository= contactInfoRepository;
        }


        [HttpGet]
        public ActionResult<IEnumerable<ContactInfoDTO>> GetInfos()
        {
            var contactInfos = _contactInfoRepository.GetAll().ToList();
            return Ok(contactInfos.ToContactInfoDTO());
        }



        [HttpGet("Contact/{contactId}", Name = "GetInfosByContactId")]
        public ActionResult<IEnumerable<ContactInfoDTO>> GetInfosByContactId(Guid contactId)
        {
            var contactInfos = _contactInfoRepository.GetContactInfosForContact(contactId);
                        
            return Ok(contactInfos.ToContactInfoDTO());
        }


        [HttpGet("{id}", Name = "GetContactInfoById")]
        public async Task<ActionResult<ContactInfoDTO>> GetContactInfoById(Guid id)
        {
            var contactInfoItem = await _contactInfoRepository.GetById(id);
            if (contactInfoItem != null)
            {
                return Ok(contactInfoItem.ToContactInfoDTO());
            }

            return NotFound();
        }


        [HttpPost]
        public async Task<ActionResult<ContactInfoDTO>> CreateContactInfo(ContactInfoDTO createValue)
        {



            var _contact = await _contactRepository.GetById(createValue.ContactUuid);

            
            if(_contact != null)
            {
                var _value = createValue.ToContactInfo();

                await _contactInfoRepository.Insert(_value);

                return CreatedAtRoute(nameof(GetContactInfoById), new { Id = _value.Uuid }, _value);


            }
            else
            {
                return NotFound("The contact is not exist");
            }


            
        }



        [HttpPut]
        public async Task<ActionResult<ContactInfoDTO>> UpdateContactInfo(ContactInfoDTO updateValue)
        {



            var _contactInfo = await _contactInfoRepository.GetById(updateValue.Uuid);


            if (_contactInfo != null)
            {



                var _contact = await _contactRepository.GetById(updateValue.ContactUuid);




                if (_contact != null)
                {

                    var _value = updateValue.ToContactInfo();

                    await _contactInfoRepository.Update(_value);
                    return CreatedAtRoute(nameof(GetContactInfoById), new { Id = _value.Uuid }, _value);


                }
                else
                {
                    return NotFound("The contact is not exist");

                }
            }
            else
            {

                return NotFound("The Contact Info is not exist for update");
            }

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<ContactInfo>> DeleteContactInfo(Guid id)
        {

            var _contactInfo = await _contactInfoRepository.GetById(id);


            if (_contactInfo != null)
            {
                await _contactInfoRepository.Delete(id);
                return Ok("Contact Info Deleted Successfully");

            }
            else
            {
                return NotFound("The Contact Info is not exist");
            }

            
        }
    }
}
