using Microsoft.EntityFrameworkCore;
using PracticeWeb.Model;
namespace PracticeWeb.DataBase
{
    public class DbManager : DbContext
    {
        private readonly string _DataBaseConnection = null!;
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
        }
    }
}