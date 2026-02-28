using FCG.Api.Domain.Enums;

namespace FCG.Api.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public UserRole Role { get; private set; }

        protected User()
        {
            Name = null!;
            Email = null!;
            PasswordHash = null!;
        }

        public User(string name, string email, string passwordHash, UserRole role)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
        }
    }
}
