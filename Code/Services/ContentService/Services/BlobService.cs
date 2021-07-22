using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContentService.Services
{
    public class BlobService : IBlobService
    {
        HttpClient _httpClient;
        public BlobService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> UploadImage(byte[] imageBytes, string extension, string token)
        {
            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            }

            ByteArrayContent bytes = new ByteArrayContent(imageBytes);

            var httpContent = new MultipartFormDataContent();
            
            httpContent.Add(bytes, "image", "Sample."+ extension);

            var httpResponse = await _httpClient.PostAsync("UploadImage", httpContent);

            return await httpResponse.Content.ReadAsStringAsync();
        }
    }
}
