using CRUD.Model;

namespace CRUD.DTO
{
    public class NotificationResultDto
    {
        public List<Notification> Notifications { get; set; }
        public int UnreadCount { get; set; }
    }
}
