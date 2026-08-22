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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
        .HasOne(u=>u.Employee)
        .WithOne(e=>e.User)
        .HasForeignKey<Employee>(e=>e.UserId);
    }
}