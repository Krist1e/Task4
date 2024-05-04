using Riok.Mapperly.Abstractions;
using Task4.Dto;
using Task4.Dto.Requests;
using Task4.Dto.Responses;
using Task4.Models;

namespace Task4.Mappers;

[Mapper]
public static partial class UserMapper
{
    [MapProperty("Password", "PasswordHash")]
    public static partial User ToEntity(this LoginRequest loginRequest);
    [MapProperty("Password", "PasswordHash")]
    public static partial User ToEntity(this RegistrationRequest registrationRequest);
    public static partial LoginResponse ToLoginResponse(this User user);
    public static partial RegistrationResponse ToRegistrationResponse(this User user);
    public static partial UserResponse ToUserResponse(this User user);
}