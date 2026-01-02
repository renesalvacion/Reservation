using CRUD.Model;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Data
{
    public class CrudDbContext : DbContext
    {
        public CrudDbContext(DbContextOptions<CrudDbContext> options) : base(options) 
        {

        }   

        public DbSet<Crud> Cruds { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<RoomCatalog> Rooms { get; set; }
        public DbSet<Review> Reviews { get; set; }
        // <-- Add this
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ImageView> Images { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

    }
}
