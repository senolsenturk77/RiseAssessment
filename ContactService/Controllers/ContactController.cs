using ContactService.Data.Entities;
using ContactService.Data.Helper;
using ContactService.Data.Repositories;
using ContactService.Data.Repositories.Interfaces;
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
    public class ContactController : ControllerBase
    {


        private readonly ILogger<ContactController> _logger;
        private readonly IContactRepository _contactRepository;

        public ContactController(ILogger<ContactController> logger, IContactRepository contactRepository)
        {

            _logger = logger;
            _contactRepository = contactRepository;
        }


       
        [HttpGet]
        public ActionResult<IEnumerable<ContactDTO>> GetContacts()
        {
            var contacts = _contactRepository.GetAll().ToList().ToContactDTO();

            return Ok(contacts);
        }



        [HttpGet("{id}", Name = "GetContactById")]
        public async Task<ActionResult<ContactDTO>> GetContactById(Guid id)
        {
            var contactItem = await _contactRepository.GetById(id);
            if (contactItem != null)
            {
                return Ok(contactItem.ToContactDTO());
            }

            return NotFound();
        }



        [HttpGet("WithInfo/{contactId}", Name = "GetContactWithInfos")]
        public async Task<ActionResult<ContactWithInfoDTO>> GetContactWithInfos(Guid contactId)
        {
            var contact= await _contactRepository.GetWithInfoById(contactId);


            if (contact != null)
            {
                return Ok(contact.ToContactWithInfoDTO());

            }
            else
            {
                return NotFound("The contact is not found.");
            }
        }


        [HttpPost]
        public async Task<ActionResult<ContactDTO>> CreateContact(ContactDTO createValue)
        {
            var _value = createValue.ToContact();
            await _contactRepository.Insert(_value);

            return CreatedAtRoute(nameof(GetContactById), new { Id = _value.Uuid }, _value);
        }



        [HttpPut]
        public async Task<ActionResult<ContactDTO>> UpdateContact(ContactDTO updateValue)
        {

            var _value = updateValue.ToContact();
            await _contactRepository.Update(_value);

            return CreatedAtRoute(nameof(GetContactById), new { Id = _value.Uuid }, _value);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Contact>> DeleteContact(Guid id)
        {

            await _contactRepository.Delete(id);

            return Ok("Contact Deleted Successfully");
        }
    }
}
