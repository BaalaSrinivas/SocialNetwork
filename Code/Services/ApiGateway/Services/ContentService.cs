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
    public class ContentService : IContentService
    {
        private HttpClient _contentHttpClient;

        public ContentService(HttpClient httpClient)
        {            
            
            _contentHttpClient = httpClient;
        }

        public async Task<IEnumerable<Comment>> GetComments(Guid postId, string token)
        {
            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            _contentHttpClient.DefaultRequestHeaders.Add("Authorization", token);

            var httpContent = new StringContent(JsonSerializer.Serialize(postId, jsonSerializerOptions), Encoding.UTF8, "application/json");

            var httpResponse = await _contentHttpClient.GetAsync($"getcomments?postid={postId}");

            return JsonSerializer.Deserialize<IEnumerable<Comment>>(await httpResponse.Content.ReadAsStringAsync(), jsonSerializerOptions);
        }

        public Task<IEnumerable<string>> GetLikedUsers(Guid parentId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Post>> GetPosts(IEnumerable<Guid> postIds, string token)
        {
            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            _contentHttpClient.DefaultRequestHeaders.Add("Authorization", token);

            var httpContent = new StringContent(JsonSerializer.Serialize(postIds, jsonSerializerOptions),Encoding.UTF8,"application/json");

            var httpResponse = await _contentHttpClient.PostAsync("getposts", httpContent);
            return JsonSerializer.Deserialize<IEnumerable<Post>>(await httpResponse.Content.ReadAsStringAsync(), jsonSerializerOptions);
        }
    }
}
