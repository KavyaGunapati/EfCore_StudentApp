using Microsoft.EntityFrameworkCore;
using StudentApp.Data;
using StudentApp.Models;
namespace StudentApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=StudentDB;Trusted_Connection=True;");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
        .Property(s => s.Name)
        .HasMaxLength(50)
        .IsRequired();
    }
}