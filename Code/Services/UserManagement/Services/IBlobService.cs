using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Services
{
    public interface IBlobService
    {
        public Task<string> UploadImage(IFormFile image, string token);
    }
}
