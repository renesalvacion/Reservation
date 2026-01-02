namespace CRUD.Services
{
    public interface INotificationService
    {
        Task NotifyUserAsync(int userId, string title, string message);
    }
}
