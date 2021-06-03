using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UserManagement.Services
{
    public class BlobService : IBlobService
    {
        HttpClient _httpClient;
        public BlobService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> UploadImage(IFormFile image, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);

            byte[] data;
            using (var br = new BinaryReader(image.OpenReadStream()))
            {
                data = br.ReadBytes((int)image.OpenReadStream().Length);
            }
            ByteArrayContent bytes = new ByteArrayContent(data);

            var httpContent = new MultipartFormDataContent();
            
            httpContent.Add(bytes, "image", image.FileName);

            var httpResponse = await _httpClient.PostAsync("UploadImage", httpContent);

            return await httpResponse.Content.ReadAsStringAsync();
        }
    }
}
