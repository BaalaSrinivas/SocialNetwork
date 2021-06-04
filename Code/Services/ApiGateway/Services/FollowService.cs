using ApiGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiGateway.Services
{
    public class FollowService : IFollowService
    {
        HttpClient _httpClient;
        public FollowService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<FriendEntity>> GetFriendRequests(string token)
        {
            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            _httpClient.DefaultRequestHeaders.Add("Authorization", token);            

            var httpResponse = await _httpClient.GetAsync("GetFriendRequests");
            return JsonSerializer.Deserialize<IEnumerable<FriendEntity>>(await httpResponse.Content.ReadAsStringAsync(), jsonSerializerOptions);
        }

        public async Task<IEnumerable<FriendEntity>> GetFriends(string token)
        {
            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            _httpClient.DefaultRequestHeaders.Add("Authorization", token);

            var httpResponse = await _httpClient.GetAsync("GetFriends");
            return JsonSerializer.Deserialize<IEnumerable<FriendEntity>>(await httpResponse.Content.ReadAsStringAsync(), jsonSerializerOptions);
        }
    }
}
