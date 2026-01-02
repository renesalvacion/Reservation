using System.ComponentModel.DataAnnotations;

namespace CRUD.Model
{
    public class ImageView
    {
        [Key]
        public int ImageId { get; set; }
        [Required]
        public string Imagepath { get; set; } 
    }
}
