using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NetProductivity.Models
{
    public class ProductivityContext : IdentityDbContext<User>
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskP> Tasks { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<UserProjects> UserProjects { get; set; }


        public ProductivityContext(DbContextOptions<ProductivityContext> options) :base(options)
        {
            Database.EnsureCreated();
        }
        public ProductivityContext()
        {
        }
    }
}
