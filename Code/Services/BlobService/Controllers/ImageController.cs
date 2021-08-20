using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace BlobService.Controllers
{
    [ApiController]
    [Route("blobapi/v1/[controller]")]
    public class ImageController : ControllerBase
    {
        public ImageController()
        {
        }

        [HttpPost]
        [Route("UploadImage")]
        public async Task<ActionResult<string>> UploadImage([FromForm] IFormFile image)
        {
            Guid guid = Guid.NewGuid();
            string imageName = $"{guid}{Path.GetExtension(image.FileName)}";
            using (Stream fileStream = new FileStream($@"{Directory.GetCurrentDirectory()}\wwwroot\Images\{imageName}", FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            using (System.Drawing.Image imageThumbnail = System.Drawing.Image.FromFile($@"{Directory.GetCurrentDirectory()}\wwwroot\Images\{imageName}"))
            {
                new Bitmap(imageThumbnail, 720, 720).Save($@"{Directory.GetCurrentDirectory()}\wwwroot\Images\{guid}_1x1{Path.GetExtension(image.FileName)}");
            }

            return Ok(@$"<BlobUrlBSK>Images/{imageName}");
        }
    }
}
