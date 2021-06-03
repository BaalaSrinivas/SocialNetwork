using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlobService.Controllers
{
    [ApiController]
    [Route("blobapi/v1/[controller]")]
    public class ImageController : ControllerBase
    {
        IConfiguration _configuration;
        public ImageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("UploadImage")]
        public async Task<ActionResult<string>> UploadImage([FromForm] IFormFile image)
        {
            string imageName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            using (Stream fileStream = new FileStream($@"{Directory.GetCurrentDirectory()}\wwwroot\Images\{imageName}", FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return Ok(@$"{_configuration.GetValue<string>("ServiceUrl")}/Images/{imageName}");
        }
    }
}
