using CRUD.DTO;
using CRUD.Model;

namespace CRUD.Services
{
    public interface IImageViewServices
    {
        Task<(string Message, bool Success)> UploadViewImage(ViewImageUpload dto, string filename);
        Task<List<ImageView>> GetUploadView();
    }
}
