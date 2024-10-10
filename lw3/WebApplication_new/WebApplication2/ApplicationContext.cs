using System.Data.Entity;

namespace lw3
{
    public partial class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("name=Model11") { }

        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) { }
    }
}
