using ApiGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiGateway.Services
{
    public class UserService : IUserService
    {
        private HttpClient _userHttpClient;

        public UserService(HttpClient httpClient)
        {
            _userHttpClient = httpClient;
        }
        public async Task<IEnumerable<SMUser>> GetUsers(IEnumerable<string> mailIds, string token)
        {
            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            _userHttpClient.DefaultRequestHeaders.Add("Authorization", token);

            var content = new StringContent(JsonSerializer.Serialize(mailIds, jsonSerializerOptions), Encoding.UTF8, "application/json");
            var response = await _userHttpClient.PostAsync("getusers", content);

            return JsonSerializer.Deserialize<IEnumerable<SMUser>>(await response.Content.ReadAsStringAsync(), jsonSerializerOptions);
        }
    }
}
