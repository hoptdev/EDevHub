using ChatService.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
