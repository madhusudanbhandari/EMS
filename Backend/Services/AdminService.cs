using Backend.Data;
using Backend.Dtos.Admin;
using Backend.Interface;
using Backend.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend.Service;

public class AdminService: IAdminService
{
    private readonly AppDbContext _context;

    public AdminService(AppDbContext context)
    {
        _context=context;
    }

    public async Task<IEnumerable<PendingUserResponseDto>> GetPendingUsersAsync()
    {
        return await _context.Users
        .AsNoTracking()
        .Where(u=>u.Status==Models.Enums.AccountStatus.PendingApproval)
        .Select(u=>new PendingUserResponseDto
        {
            Id=u.Id,
            Name=u.Name,
            Email=u.Email,
            Status=u.Status
        }).ToListAsync();

     
    }

    public async Task<bool> ApproveUserAsync(int userId,
    UserRole role)
    {
        var user=await _context.Users
        .FirstOrDefaultAsync(u=>u.Id==userId);

        if (user == null)
        {
            return false;
        }
        if (user.Status != AccountStatus.PendingApproval)
        {
            return false;
        }

        if (role == UserRole.None || role == UserRole.Admin)
        {
            return false;
        }

        user.Role=role;

        user.Status=AccountStatus.Approved;
        await _context.SaveChangesAsync();
        return true;

    }

    public async Task<bool> RejectUserAsync(int userId)
    {
        var user=await _context.Users
        .FirstOrDefaultAsync(u=>u.Id==userId);

        if (user == null)
        {
            return false;
        }
        if (user.Status != AccountStatus.PendingApproval)
        {
            return false;
        }

        user.Status=AccountStatus.Rejected;
        await _context.SaveChangesAsync();
        return true;
    }
}