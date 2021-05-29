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
        public async Task<IEnumerable<string>> GetUserFollowingAsync(string userId, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);

            var httpResponse = await _httpClient.GetAsync("GetFollowing");
            //TODO: Make it asynchronous
            return JsonSerializer.Deserialize<IEnumerable<string>>(await httpResponse.Content.ReadAsStringAsync());
        }
    }
}
