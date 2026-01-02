using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Model
{
    // Enum for message type
    public enum MessageType
    {
        Text,
        Image,
        File,
        Emoji
    }

    // Enum for message status
    public enum MessageStatus
    {
        Sent,
        Delivered,
        Read
    }

    [Table("Messenger")]
    public class Messenger
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }  // Unique message ID

        [Required]
        public Guid SenderId { get; set; }  // User who sent the message

        [Required]
        public Guid ReceiverId { get; set; }  // Recipient user or group

        [Required]
        public Guid ConversationId { get; set; }  // Conversation or thread ID

        [Required]
        public string Message { get; set; }  // Message content

        [Required]
        public MessageType MessageType { get; set; } = MessageType.Text;  // Type of message

        [Required]
        public MessageStatus Status { get; set; } = MessageStatus.Sent;  // Delivery status

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // When message was sent

        public DateTime? UpdatedAt { get; set; }  // Last updated time for edits

        public bool IsDeleted { get; set; } = false;  // Soft delete flag
    }
}
