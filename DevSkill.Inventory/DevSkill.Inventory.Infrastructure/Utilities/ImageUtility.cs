using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DevSkill.Inventory.Infrastructure.Utilities
{
    public class ImageUtility : IImageUtility
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ImageUtility(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<string> UploadImage(IFormFile image, string imageUrl)
        {
            if (image != null)
            {
                // Get file extension (default to .jpg if missing)
                var extension = Path.GetExtension(image.FileName);
                if (string.IsNullOrWhiteSpace(extension))
                {
                    extension = ".jpg";
                }

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                return fileName; 
            }

            return imageUrl;
        }

    }
}
