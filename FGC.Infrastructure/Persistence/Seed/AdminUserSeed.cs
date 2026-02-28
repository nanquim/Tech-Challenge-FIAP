using FCG.Api.Domain.Entities;
using FCG.Api.Domain.Enums;
using FCG.Api.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;

namespace FCG.Api.Infrastructure.Persistence.Seed
{
    public static class AdminUserSeed
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.Users.Any(u => u.Role == UserRole.Admin))
                return;

            var hasher = new PasswordHasher<string>();

            var admin = new User(
                "Admin",
                "admin@fcg.com",
                hasher.HashPassword(null!, "Admin@123"),
                UserRole.Admin
            );

            context.Users.Add(admin);
            context.SaveChanges();
        }
    }
}
