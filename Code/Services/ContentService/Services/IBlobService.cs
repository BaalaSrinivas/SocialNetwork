using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentService.Services
{
    public interface IBlobService
    {
        public Task<string> UploadImage(byte[] imageBytes, string extension, string token);
    }
}
