using service.Models;
using System.Data.Entity;

namespace service
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<Student> Students { get; set; }
    }
}