using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Task4.Data;
using Task4.Dto.Responses;
using Task4.Mappers;
using Task4.Models;

namespace Task4.Services;

public class UserService
{
    private readonly UserManager<User> _userManager;
    
    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ErrorOr<UserResponse>> ChangeUserStatusAsync(Guid userId, Status status)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return Error.NotFound(description: "User not found.");
        }

        user.Status = status;
        await _userManager.UpdateAsync(user);

        return user.ToUserResponse();
    }
    
    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return;
        }

        await _userManager.DeleteAsync(user);
    }
    
    public async Task<ErrorOr<UserResponse>> GetUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user == null ? Error.NotFound(description: "User not found.") : user.ToUserResponse();
    }
    
    public async Task<ErrorOr<IEnumerable<UserResponse>>> GetUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return users.Select(user => user.ToUserResponse()).ToList();
    }
    
}