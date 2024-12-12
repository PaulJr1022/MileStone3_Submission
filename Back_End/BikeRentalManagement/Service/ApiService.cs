using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BikeRentalManagement.Service
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CallApis()
        {
            // Example API call
            var response = await _httpClient.PutAsync("http://localhost:5102/api/Rent/LateReturns", new StringContent(string.Empty));
            var responseReminder=await _httpClient.GetAsync("http://localhost:5102/api/Rent/Reminder");

            if (response.IsSuccessStatusCode && responseReminder.IsSuccessStatusCode)
            {
                Console.WriteLine("API call succeeded.");
            }
            else
            {
                Console.WriteLine("API call failed.");
            }
        }
    }
}
