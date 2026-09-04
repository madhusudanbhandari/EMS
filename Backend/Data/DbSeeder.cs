

using Backend.Data;
using Backend.Models;
using Backend.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;
public static class DbSeeder
{
    public  static async Task SeedAdminAsync(AppDbContext context,IConfiguration Configuration)
    {
        var adminExist=await context.Users
        .AnyAsync(u=>u.Role==UserRole.Admin);

        if (adminExist)
        {
            return;
        }

        var adminEmail=Configuration["SeedAdmin:Email"];
        var adminPassword=Configuration["SeedAdmin:Password"];

        var admin=new User
        {
            Name="Madhusudan Bhandari",
            Email=adminEmail ?? throw new InvalidOperationException("SeedAdmin:Email is not configured."),
            PasswordHash=BCrypt.Net.BCrypt.HashPassword(adminPassword),
            Role=UserRole.Admin,
            Status=AccountStatus.Approved,
            CreatedAt=
            DateTime.UtcNow
        };

        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}