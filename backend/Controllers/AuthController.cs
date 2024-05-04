using System.Security.Claims;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task4.Dto;
using Task4.Dto.Requests;
using Task4.Dto.Responses;
using Task4.Mappers;
using Task4.Models;
using Task4.Services;

namespace Task4.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;

    private readonly AuthService _authService;

    public AuthController(ILogger<AuthController> logger, UserManager<User> userManager, AuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<User>> Register([FromBody] RegistrationRequest registrationRequest,
        [FromServices] IValidator<RegistrationRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(registrationRequest);
        
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var result = await _authService.RegisterAsync(registrationRequest);
        return result.MatchFirst<ActionResult<User>>(
            user => Ok(user),
            error => UnprocessableEntity(new ErrorResponse(error.Description))
        );
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<User>> Login([FromBody] LoginRequest loginRequest)
    {
        var result = await _authService.LoginAsync(loginRequest);
        return result.MatchFirst<ActionResult<User>>(
            user => Ok(user),
            error => Unauthorized(new ErrorResponse(error.Description))
        );
    }
    
    [HttpPost]
    [Route("logout")]
    [Authorize]
    public async Task<ActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return Ok();
    }
}