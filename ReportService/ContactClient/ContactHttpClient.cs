using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ReportService.ContactClient.Interfaces;
using ReportService.Data.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReportService.ContactClient
{
    public class ContactHttpClient: IContactClient
    {


        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ContactHttpClient(HttpClient client, IConfiguration configuration)
        {
            _httpClient = client;
            _configuration = configuration;
      

        }


        public async Task<IEnumerable<ContactInfoDTO>> GetAllContactInfos()
        {
            List<ContactInfoDTO> contactInformationList = new List<ContactInfoDTO>();
            var response = await _httpClient.GetAsync($"{_configuration["ContactService"]}/api/ContactInfo");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                contactInformationList = JsonConvert.DeserializeObject<List<ContactInfoDTO>>(responseData);
            }
            return contactInformationList;
        }
    }
}
