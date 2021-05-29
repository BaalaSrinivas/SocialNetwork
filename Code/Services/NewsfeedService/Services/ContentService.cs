using NewsfeedService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NewsfeedService.Services
{
    public class ContentService : IContentService
    {
        HttpClient _httpClient;
        public ContentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Guid>> GetUsersPosts(UserPostDTO userPostDTO, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var httpContent = new StringContent(JsonSerializer.Serialize(userPostDTO), Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PostAsync("getuserposts", httpContent);
            //TODO: Make it asynchronous
            return JsonSerializer.Deserialize<IEnumerable<Guid>>(await httpResponse.Content.ReadAsStringAsync());
        }
    }
}
