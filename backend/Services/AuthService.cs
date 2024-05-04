using System.Security.Authentication;
using ErrorOr;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task4.Dto;
using Task4.Dto.Requests;
using Task4.Dto.Responses;
using Task4.Mappers;
using Task4.Models;

namespace Task4.Services;

public class AuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IValidator<LoginRequest> _loginRequestValidator;
    private readonly IValidator<RegistrationRequest> _registrationRequestValidator;

    public AuthService(UserManager<User> userManager, IValidator<LoginRequest> validator,
        IValidator<RegistrationRequest> registrationRequestValidator, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _loginRequestValidator = validator;
        _registrationRequestValidator = registrationRequestValidator;
        _signInManager = signInManager;
    }

    public async Task<ErrorOr<RegistrationResponse>> RegisterAsync(RegistrationRequest request)
    {
        var validationResult = await _registrationRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Error.Validation(description: "Invalid email, password or username.");
        }
        
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return Error.Conflict(description: "User with this email already exists.");
        }

        User user = request.ToEntity();
        user.UserName = request.Email;
        
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return Error.Unexpected(description: "User creation failed! Please check user details and try again.");
        }

        return user.ToRegistrationResponse();
    }

    public async Task<ErrorOr<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var validationResult = await _loginRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return Error.Validation(description: "Invalid email or password.");
        }

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Error.NotFound(description: "User not found.");

        if (user.Status == Status.Blocked)
        {
            return Error.Unauthorized(description: "User is blocked.");
        }

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
        if (!result.Succeeded)
        {
            return Error.Unauthorized(description: "Invalid email or password.");
        }

        return user.ToLoginResponse();
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}