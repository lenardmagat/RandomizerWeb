// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;

// namespace PracticeWeb.DataBase
// {
//     // This interface tells EF Core: "Ignore Program.cs, build DbManager from here"
//     public class DbManagerFactory : IDesignTimeDbContextFactory<DbManager>
//     {
//         public DbManager CreateDbContext(string[] args)
//         {
//             var optionsBuilder = new DbContextOptionsBuilder<DbManager>();
            
//             // Your Docker Postgres connection string
//             optionsBuilder.UseNpgsql("Host=localhost;Port=5434;Database=WebPractice;Username=postgres;Password=lenlen");

//             return new DbManager();
//         }
//     }
// }