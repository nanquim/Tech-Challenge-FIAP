using Microsoft.AspNetCore.Identity;

namespace FCG.Api.Application.Security
{
    public static class PasswordHasher
    {
        private static readonly PasswordHasher<string> _hasher = new();

        public static string Hash(string password)
        {
            return _hasher.HashPassword(null!, password);
        }
    }
}
