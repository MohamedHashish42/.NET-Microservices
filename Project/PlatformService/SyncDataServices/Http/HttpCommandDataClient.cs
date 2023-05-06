using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : IHttpCommandDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public HttpCommandDataClient(IConfiguration configuration,HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task SendRequestToCommandsService(PlatformReadDto Platform)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(Platform), 
                Encoding.UTF8, 
                "application/json");
            var response =await _httpClient.PostAsync(_configuration["CommandsService:TestConnection"],httpContent);

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sending Http Request To CommandsService Was OK!");
            }
            else
            {
                Console.WriteLine("--> Sending Http Request To CommandsService Was Not OK!");
            }                
        }
    }
}