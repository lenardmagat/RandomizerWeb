using Microsoft.EntityFrameworkCore;
using PracticeWeb.Model;
namespace PracticeWeb.DataBase
{
    public class DbManager : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<Member> Members { get; set; } = null!;
        public DbSet<GroupMember> GroupMembers { get; set; } = null!;
        public  DbManager(DbContextOptions<DbManager> options) : base(options){}//(string DataBaseConnection) => _DataBaseConnection = DataBaseConnection;
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name)
                .IsUnique();
            modelBuilder.Entity<Group>().HasIndex(g => g.UserId);
            modelBuilder.Entity<GroupMember>().HasIndex(m => m.GroupId);
        }
    }
}