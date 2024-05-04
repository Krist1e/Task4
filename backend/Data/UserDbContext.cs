using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task4.Models;

namespace Task4.Data;

public class UserDbContext(DbContextOptions<UserDbContext> options)
    : IdentityUserContext<User, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>()
            .Ignore(u => u.EmailConfirmed)
            .Ignore(u => u.LockoutEnabled)
            .Ignore(u => u.LockoutEnd)
            .Ignore(u => u.PhoneNumber)
            .Ignore(u => u.AccessFailedCount)
            .Ignore(u => u.TwoFactorEnabled)
            .Ignore(u => u.PhoneNumberConfirmed);

        builder.Entity<User>(b => { b.ToTable("Users"); });

        builder.Entity<IdentityUserClaim<Guid>>(b => { b.ToTable("UserClaims"); });

        builder.Entity<IdentityUserLogin<Guid>>(b => { b.ToTable("UserLogins"); });

        builder.Entity<IdentityUserToken<Guid>>(b => { b.ToTable("UserTokens"); });
    }
}