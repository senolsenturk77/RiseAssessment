using Castle.Core.Logging;
using ContactService.Controllers;
using ContactService.Data.Entities;
using ContactService.Data.Repositories;
using ContactService.Data.Helper;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using ContactService.Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace ContactService.Test
{
    public class ContactControllerTest
    {

        private readonly Mock<IContactRepository> _contactRepository;
        private readonly Mock<IContactInfoRepository> _contactInfoRepository;
        private readonly Mock<ILogger<ContactController>> _logger;


        private readonly ContactController _contactContoller;

        

        private readonly List<Contact> _contactReadList;


        public ContactControllerTest()
        {

            _contactRepository = new Mock<IContactRepository>();
            _contactInfoRepository = new Mock<IContactInfoRepository>();
            _logger = new Mock<ILogger<ContactController>>();


            _contactReadList = new List<Contact> {
            new Contact(){Name = "Þenol",Surename = "Þentürk",CompanyName = "Ear",  Uuid = Guid.Parse("18c1d7f9-9704-459d-8424-c36e00f3b147")},
            new Contact(){Name = "Barýþ",Surename = "Yýlmaz",CompanyName = "Siser", Uuid= Guid.Parse("34bdbacd-74d1-4012-8d73-dbdd56512de9")} };

            _contactContoller = new ContactController(_logger.Object, _contactRepository.Object);

        }




        [Fact]
        public void GetAllContact_Test()
        {

            try
            {



                _contactRepository.Setup(x => x.GetAll()).Returns(_contactReadList.AsQueryable());

             


                var result = _contactContoller.GetContacts();


                var responseContact = result.Result as OkObjectResult;
                var list = Assert.IsAssignableFrom<IEnumerable<Contact>>((responseContact.Value as IEnumerable<ContactDTO>).ToContact());


                Assert.Equal<int>(_contactReadList.Count, list.Count());

            }
            catch (Exception ex)
            {

                Assert.False(false, ex.Message);
            }
        }





        [Fact]
        public async Task GetContactByID()
        {

            try
            {

                var contact = new Contact() { Name = "Barýþ", Surename = "Yýlmaz", CompanyName = "Siser", Uuid = Guid.Parse("34bdbacd-74d1-4012-8d73-dbdd56512de9") } ;




            _contactRepository.Setup(x => x.GetById(contact.Uuid)).ReturnsAsync(contact);

          


            var result = await _contactContoller.GetContactById(contact.Uuid);


                Assert.Equal(contact.Uuid, result.Value.Uuid);
                Assert.Equal(contact.Name, result.Value.Name);

            }
            catch (Exception ex)
            {

                Assert.False(false, ex.Message);
            }
}



        [Fact]
        public async Task GetContactWithInfos()
        {

            try
            {

                var contact = new Contact() { Name = "Barýþ", Surename = "Yýlmaz", CompanyName = "Siser", Uuid = Guid.Parse("34bdbacd-74d1-4012-8d73-dbdd56512de9"),
                    ContactInformations = new List<ContactInfo>()
                };

                contact.ContactInformations.Add(new ContactInfo() { Uuid= Guid.NewGuid(), Contact=contact, ContactUuid=contact.Uuid,
                    InfoType= ContactInfoType.Location, Info="Çorlu"});



                _contactRepository.Setup(x => x.GetWithInfoById(contact.Uuid)).ReturnsAsync(contact);

              
                var result = await _contactContoller.GetContactWithInfos(contact.Uuid);


                Assert.Equal(contact.Uuid, result.Value.Uuid);
                Assert.Equal(contact.ContactInformations.Count, result.Value.ContactInformations.Count);

            }
            catch (Exception ex)
            {

                Assert.False(false, ex.Message);
            }
        }




        [Fact]
        public async Task CreateContact()
        {

            

                var contactdto = new ContactDTO()
                {
                    Name = "Barýþ",
                    Surename = "Yýlmaz",
                    CompanyName = "Siser",
                    Uuid = Guid.Parse("34bdbacd-74d1-4012-8d73-dbdd56512de9"),
                };


                var contact = contactdto.ToContact();

                _contactRepository.Setup(x => x.Insert(contact));

           


                var result = await _contactContoller.CreateContact(contactdto);

                Assert.IsType<CreatedAtRouteResult>(result.Result);

                var newcontact = ((CreatedAtRouteResult)result.Result);


                Assert.Equal(201, newcontact.StatusCode);
             //   Assert.Equal("Barýþ", result.Value.Name);

           
        }




        [Fact]
        public async Task UpdateContact()
        {



            var contactdto = new ContactDTO()
            {
                Name = "Barýþ",
                Surename = "Yýlmaz",
                CompanyName = "Siser",
                Uuid = Guid.Parse("34bdbacd-74d1-4012-8d73-dbdd56512de9"),
            };


            var contact = contactdto.ToContact();

            _contactRepository.Setup(x => x.Update(contact));

          


            var result = await _contactContoller.UpdateContact(contactdto);

            Assert.IsType<CreatedAtRouteResult>(result.Result);

            var newcontact = ((CreatedAtRouteResult)result.Result);


            Assert.Equal(201, newcontact.StatusCode);
            //   Assert.Equal("Barýþ", result.Value.Name);


        }

    }
}
