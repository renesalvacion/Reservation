using CRUD.Data;
using CRUD.DTO;
using CRUD.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace CRUD.Services
{
    public class ImageViewServices : IImageViewServices
    {
        private readonly CrudDbContext _context;
        public ImageViewServices(CrudDbContext context) { 
            _context =context;
        }

        public async Task<(string Message, bool Success)> UploadViewImage(ViewImageUpload dto, string filename)
        {
            if (dto == null) return ("Dto is Null", false);

            var image = new ImageView
            {
                Imagepath = filename,
            };

             _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return ("Upload Image SuccessFully", true);
        }

        public async Task<List<ImageView>> GetUploadView()
        {
            return await _context.Images
                .OrderByDescending(x => x.ImageId)
                .ToListAsync();
        }
    }
}
