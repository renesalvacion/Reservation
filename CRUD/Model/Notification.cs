using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Model
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to user
        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public Crud User { get; set; } = null!;  // Navigation property to user

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        // Mark if notification has been read
        public bool IsRead { get; set; } = false;

        // Track when notification was created
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
