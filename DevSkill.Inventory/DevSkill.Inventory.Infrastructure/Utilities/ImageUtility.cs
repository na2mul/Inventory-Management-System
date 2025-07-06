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
                var fileName = $"{Guid.NewGuid()}-{Path.GetFileName(image.FileName)}";
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                imageUrl = Path.Combine("images", fileName);

                return imageUrl;
            }
            return imageUrl;
        }
    }
}
