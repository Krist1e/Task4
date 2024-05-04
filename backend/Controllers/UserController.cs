using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task4.Dto.Responses;
using Task4.Models;
using Task4.Services;

namespace Task4.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    public UserController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> GetUsers()
    {
        var result = await _userService.GetUsersAsync();
        return result.MatchFirst<ActionResult>(
            users => Ok(users),
            error => NotFound(new ErrorResponse(error.Description))
        );
    }
    
    [HttpGet]
    [Route("{userId:guid}")]
    [Authorize]
    public async Task<ActionResult> GetUser(Guid userId)
    {
        var result = await _userService.GetUserAsync(userId);
        return result.MatchFirst<ActionResult>(
            user => Ok(user),
            error => NotFound(new ErrorResponse(error.Description))
        );
    }
    
    [HttpDelete]
    [Route("{userId:guid}")]
    [Authorize]
    public async Task<ActionResult> DeleteUser(Guid userId)
    {
        await _userService.DeleteUserAsync(userId);
        return NoContent();
    }
    
    [HttpPatch]
    [Route("{userId:guid}/status")]
    [Authorize]
    public async Task<ActionResult> ChangeUserStatus(Guid userId, [FromBody] Status status)
    {
        var result = await _userService.ChangeUserStatusAsync(userId, status);
        return result.MatchFirst<ActionResult>(
            user => Ok(user),
            error => NotFound(new ErrorResponse(error.Description))
        );
    }
    
}