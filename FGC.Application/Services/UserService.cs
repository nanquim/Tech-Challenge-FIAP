using FCG.Api.Domain.Entities;
using FCG.Api.Domain.Enums;
using FCG.Api.Domain.Repositories;
using FCG.Api.Domain.ValueObjects;
using FCG.Api.Application.DTOs;
using FCG.Api.Application.Validators;
using FCG.Api.Application.Security;

namespace FCG.Api.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> CreateAsync(CreateUserRequest request)
        {
            var email = new Email(request.Email);

            if (!PasswordValidator.IsValid(request.Password))
                throw new ArgumentException("Senha inválida");

            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                throw new ArgumentException("Email já cadastrado");

            var passwordHash = PasswordHasher.Hash(request.Password);

            var user = new User(request.Name, email, passwordHash, UserRole.User);

            await _userRepository.AddAsync(user);

            return user.Id;
        }
    }
}
