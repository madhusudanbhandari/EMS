using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options){

    }

    public DbSet<Employee> Employees{get;set;}
    public DbSet<Department> Departments{get;set;}
    public DbSet<User> Users{get;set;}

    public DbSet<Attendence> Attendences{get;set;}
    public DbSet<Leave> Leaves{get;set;}
    public DbSet<Payroll> Payrolls{get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
        .HasOne(u=>u.Employee)
        .WithOne(e=>e.User)
        .HasForeignKey<Employee>(e=>e.UserId);

        modelBuilder.Entity<Attendence>()
        .HasOne(a=>a.Employee)
        .WithMany(e=>e.Attendences)
        .HasForeignKey(a=>a.EmployeeId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Attendence>()
        .HasIndex(a=>new {a.EmployeeId, a.Date})
        .IsUnique();

        modelBuilder.Entity<Leave>()
        .HasOne(l=>l.Reviewer)
        .WithMany()
        .HasForeignKey(l=>l.ReviewedBy)
        .OnDelete(DeleteBehavior.Restrict);
    }
}