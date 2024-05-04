using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Task4.Models;

[Table("users")]
[Index(nameof(Email), IsUnique = true)]
public class User : IdentityUser<Guid>
{
    [MaxLength(256)] public string Name { get; set; } = string.Empty;
    public DateTime LastLoginTime { get; set; } = DateTime.UtcNow;
    public DateTime RegistrationTime { get; init; } = DateTime.UtcNow;
    public Status Status { get; set; } = Status.Active;
}