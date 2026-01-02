using CRUD.DTO;
using CRUD.Services;
using CRUD.Services.FileValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        private readonly IFileValidationServices _fileValid;
        private readonly IImageViewServices _imageViewServices;
        public ImageUploadController(IFileValidationServices fileValid, IImageViewServices imageServices) 
        { 
            _fileValid = fileValid;
            _imageViewServices = imageServices;
        }


        [HttpPost]
        public async Task<IActionResult> UploadImageView([FromForm] ViewImageUpload dto)
        {
            var valid = await _fileValid.FileValidation(dto.UploadImage, [".png", ".jpg", ".avif" ],"UploadViewProfile");

            if (!valid.Success) return BadRequest(new { status = valid.Success, message = valid.Message });

            string uniqueFilename = valid.Message;

            var result = await _imageViewServices.UploadViewImage(dto, uniqueFilename);

            if (!result.Success) return BadRequest(new { status = result.Success, message = result.Message });

            return Ok(new { status = result.Success, message = result.Message });

        }

        [HttpGet]
        public async Task<IActionResult> GetImage()
        {
            try
            {
                var result = await _imageViewServices.GetUploadView();
                return Ok(result);
            }
            catch (Exception ex)
            {
                   return BadRequest(ex);
            }
        }
    }
}
