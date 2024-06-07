using Newtonsoft.Json;
using System.Text;

namespace BankAccount.Service
{
    public class InvokeApi
    {
        private readonly HttpClient _httpClient;

        public InvokeApi(HttpClient httpClient, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClient;
            _httpClient = httpClientFactory.CreateClient("NamedClient");
        }

        public async Task<string> SendOrderdetails(string baseUrl, orderModel order)
        {
            var json = JsonConvert.SerializeObject(order);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{baseUrl}/Logistic/AddOrder", content);

            if (response.IsSuccessStatusCode)
            {
                return "Order details sent successfully";
            }
            else
            {
                return "Error sending the target API";
            }
        }
    }
}
