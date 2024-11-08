using StudentsModel;
using System.Data.Entity;

namespace PWS_Lab6
{
    public partial class ApplicationContext : DbContext
    {
        public ApplicationContext()
            : base("name=ApplicationContext")
        {
        }

        public virtual DbSet<note> Note { get; set; }
        public virtual DbSet<student> Student { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<student>()
                .HasMany(e => e.note)
                .WithOptional(e => e.student)
                .HasForeignKey(e => e.stud_id);
        }
    }
}
