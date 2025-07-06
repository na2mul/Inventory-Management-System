using Microsoft.AspNetCore.Http;

namespace DevSkill.Inventory.Infrastructure.Utilities
{
    public interface IImageUtility
    {
        public Task<string> UploadImage(IFormFile image, string imageUrl);
    }
}
