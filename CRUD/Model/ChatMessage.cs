namespace CRUD.Model
{
    public class ChatMessage
    {
        public int Id { get; set; }

        public string SenderId { get; set; } = null!;
        public string SenderName { get; set; } = null!;
        public string SenderRole { get; set; } = null!;

        public int? RecipientId { get; set; }  // null = broadcast / admin

        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
