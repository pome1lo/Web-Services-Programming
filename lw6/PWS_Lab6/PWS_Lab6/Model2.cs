using StudentsModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace PWS_Lab6
{
    public partial class Model2 : DbContext
    {
        public Model2()
            : base("name=Model2")
        {
        }

        public virtual DbSet<note> note { get; set; }
        public virtual DbSet<student> student { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<student>()
                .HasMany(e => e.note)
                .WithOptional(e => e.student)
                .HasForeignKey(e => e.stud_id);
        }
    }
}
