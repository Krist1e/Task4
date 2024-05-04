using Task4.Models;

namespace Task4.Dto.Responses;

public record UserResponse(Guid Id, string Email, string Name, DateTime LastLoginTime, Status Status);