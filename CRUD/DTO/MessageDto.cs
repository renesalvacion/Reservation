namespace CRUD.DTO
{
    public class MessageDto
    {
        public string Content { get; set; } = string.Empty;
        public int? RecipientId { get; set; } // null = broadcast
    }
}
