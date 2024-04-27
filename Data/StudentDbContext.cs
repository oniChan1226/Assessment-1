using Microsoft.EntityFrameworkCore;
using StudentPortal.API.Models.Domain;

namespace StudentPortal.API.Data
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options): base(options)
        {
            
        }
        public DbSet<StudentDomainModel> Students {  get; set; }
        public DbSet<AddressDomainModel> Addresses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentDomainModel>().HasKey(s => s.RollNumber);
            modelBuilder.Entity<AddressDomainModel>().HasKey(a => a.Id);

            modelBuilder.Entity<AddressDomainModel>()
                .HasOne<StudentDomainModel>(a => a.Student)
                .WithMany(s => s.Addresses)
                .HasForeignKey(a => a.StudentId);
        }

    }
}
