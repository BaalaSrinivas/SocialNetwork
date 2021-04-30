using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace NewsfeedService.Services
{
    public class FollowService : IFollowService
    {
        HttpClient _httpClient;
        public FollowService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<string>> GetUserFollowersAsync(string userId)
        {
            var httpResponse = await _httpClient.GetAsync($"GetFollowers?userId={userId}");
            //TODO: Make it asynchronous
            return JsonSerializer.Deserialize<IEnumerable<string>>(httpResponse.Content.ReadAsStringAsync().Result);
        }
    }
}
