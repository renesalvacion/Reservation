using Microsoft.AspNetCore.Mvc;

namespace CRUD.DTO
{
    public class CrudDto
    {
       
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        [FromForm(Name = "isActive")]
        public bool IsActive { get; set; } = true;
        public IFormFile? Profile { get; set; }
    }
}

