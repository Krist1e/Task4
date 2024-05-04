using Task4.Models;

namespace Task4.Dto.Responses;

public record LoginResponse(
    Guid Id,
    string Email,
    string Name,
    DateTime RegistrationTime,
    DateTime LastLoginTime,
    Status Status
);