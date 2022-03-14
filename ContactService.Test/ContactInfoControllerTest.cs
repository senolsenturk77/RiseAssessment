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
    public class ContactInfoControllerTest
    {

        private readonly Mock<IContactRepository> _contactRepository;
        private readonly Mock<IContactInfoRepository> _contactInfoRepository;
        private readonly Mock<ILogger<ContactInfoController>> _logger;


        private readonly ContactInfoController _contactContoller;



        private readonly List<ContactInfo> _contactReadList;


        public ContactInfoControllerTest()
        {

            _contactRepository = new Mock<IContactRepository>();
            _contactInfoRepository = new Mock<IContactInfoRepository>();
            _logger = new Mock<ILogger<ContactInfoController>>();


            _contactReadList = new List<ContactInfo> {
            new ContactInfo(){ContactUuid=Guid.Parse("b2a95846-9d34-4807-beb5-6796153e0823"), Uuid = Guid.Parse("18c1d7f9-9704-459d-8424-c36e00f3b147"),
             InfoType= ContactInfoType.Location, Info="Çorlu"},

            new ContactInfo(){ContactUuid=Guid.Parse("605b1309-13a8-4d47-9d15-b225de0a3c7a"), Uuid = Guid.Parse("34c1189e-0d2e-4f06-8eae-ae2ed8791455"),
             InfoType= ContactInfoType.Location, Info="Ankara"},

                new ContactInfo(){ContactUuid=Guid.Parse("605b1309-13a8-4d47-9d15-b225de0a3c7a"), Uuid = Guid.Parse("bf08e686-6a40-488e-bc95-3fffe33b0ac5"),
             InfoType= ContactInfoType.Location, Info="Istanbul"}};

            _contactContoller = new ContactInfoController(_logger.Object, _contactRepository.Object, _contactInfoRepository.Object);

        }




        [Fact]
        public void GetAllContact_Test()
        {

          


                _contactInfoRepository.Setup(x => x.GetAll()).Returns(_contactReadList.AsQueryable());




                var result = _contactContoller.GetInfos();


                var responseContact = result.Result as OkObjectResult;
                var list = Assert.IsAssignableFrom<IEnumerable<ContactInfoDTO>>((responseContact.Value as IEnumerable<ContactInfoDTO>));

                Assert.Equal<int>(_contactReadList.Count, list.Count());

          
        }





        [Fact]
        public async Task GetContactInfoByID()
        {

          
                var contact = new ContactInfo()
                {
                    ContactUuid = Guid.Parse("605b1309-13a8-4d47-9d15-b225de0a3c7a"),
                    Uuid = Guid.Parse("bf08e686-6a40-488e-bc95-3fffe33b0ac5"),
                    InfoType = ContactInfoType.Location,
                    Info = "Istanbul"
                };



            _contactInfoRepository.Setup(x => x.GetById(contact.Uuid)).ReturnsAsync(contact);




                var result = await _contactContoller.GetContactInfoById(contact.Uuid);
                var responseContact = result.Result as OkObjectResult;

            Assert.Equal(contact.Uuid, (responseContact.Value as ContactInfoDTO).Uuid );
                Assert.Equal( ContactInfoType.Location, (responseContact.Value as ContactInfoDTO).InfoType);

          
        }



        [Fact]
        public void GetContactInfos()
        {

            try
            {

                var _contactReadList2 = new List<ContactInfo> {
            new ContactInfo(){ContactUuid=Guid.Parse("605b1309-13a8-4d47-9d15-b225de0a3c7a"), Uuid = Guid.Parse("34c1189e-0d2e-4f06-8eae-ae2ed8791455"),
             InfoType= ContactInfoType.Location, Info="Ankara"},

                new ContactInfo(){ContactUuid=Guid.Parse("605b1309-13a8-4d47-9d15-b225de0a3c7a"), Uuid = Guid.Parse("bf08e686-6a40-488e-bc95-3fffe33b0ac5"),
             InfoType= ContactInfoType.Location, Info="Istanbul"}};

                var uuid = Guid.Parse("605b1309-13a8-4d47-9d15-b225de0a3c7a");






                _contactInfoRepository.Setup(x => x.GetContactInfosForContact(uuid)).Returns(_contactReadList2);


                var result = _contactContoller.GetInfosByContactId(uuid);


                Assert.Equal(_contactReadList2.Count, result.Value.Count());
                

            }
            catch (Exception ex)
            {

                Assert.False(false, ex.Message);
            }
        }




        [Fact]
        public async Task CreateContactInfo()
        {

            var contactInfoDto = new ContactInfoDTO()
            {
                ContactUuid = Guid.Parse("605b1309-13a8-4d47-9d15-b225de0a3c7a"),
                Uuid = Guid.Parse("bf08e686-6a40-488e-bc95-3fffe33b0ac5"),
                InfoType = ContactInfoType.Location,
                Info = "Istanbul"
            };


            var contactInfo = contactInfoDto.ToContactInfo();

            _contactInfoRepository.Setup(x => x.Insert(contactInfo));


            var contact = new Contact() { Name = "Barış", Surename = "Yılmaz", CompanyName = "Siser", Uuid = Guid.Parse("34bdbacd-74d1-4012-8d73-dbdd56512de9") };

            _contactRepository.Setup(x => x.GetById(contact.Uuid)).ReturnsAsync(contact);



            var result = await _contactContoller.CreateContactInfo(contactInfoDto);



        }



    }
}
