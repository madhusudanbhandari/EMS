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

    public DbSet<Message> Messages{get;set;}
    public DbSet<Conversation> Conversations {get;set;}
    public DbSet<ConversationParticipant>ConversationParticipants{get;set;} 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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

        modelBuilder.Entity<ConversationParticipant>()
        .HasOne(cp=>cp.Conversation)
        .WithMany(c=>c.Participants)
        .HasForeignKey(cp=>cp.ConversationId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Message>()
        .HasOne(m=>m.Conversation)
        .WithMany(c=>c.Messages)
        .HasForeignKey(m=>m.ConversationId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}