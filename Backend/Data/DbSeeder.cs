

using Backend.Data;
using Backend.Models;
using Backend.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;
public static class DbSeeder
{
    public  static async Task SeedAdminAsync(AppDbContext context)
    {
        var adminExist=await context.Users
        .AnyAsync(u=>u.Role==UserRole.Admin);

        if (adminExist)
        {
            return;
        }

        var admin=new User
        {
            Name="Madhusudan Bhandari",
            Email="madhusudanb636@gmail.com",
            PasswordHash=BCrypt.Net.BCrypt.HashPassword("madhu@123"),
            Role=UserRole.Admin,
            Status=AccountStatus.Approved,
            CreatedAt=
            DateTime.UtcNow
        };

        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}