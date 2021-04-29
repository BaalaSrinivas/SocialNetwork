using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        public async Task<IEnumerable<Guid>> GetUsersPosts(IEnumerable<string> userIds, int count)
        {
            var httpResponse = await _httpClient.PostAsync("", new StringContent(JsonSerializer.Serialize(userIds)));
            //TODO: Make it asynchronous
            return JsonSerializer.Deserialize<IEnumerable<Guid>>(httpResponse.Content.ReadAsStringAsync().Result);
        }
    }
}
