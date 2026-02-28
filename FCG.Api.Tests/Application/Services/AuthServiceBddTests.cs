using FluentAssertions;
using Moq;
using Microsoft.Extensions.Configuration;
using FCG.Api.Application.DTOs;
using FCG.Api.Application.Security;
using FCG.Api.Application.Services;
using FCG.Api.Domain.Entities;
using FCG.Api.Domain.Enums;
using FCG.Api.Domain.Repositories;
using FCG.Api.Domain.ValueObjects;

namespace FCG.Api.Tests.Application.Services
{
    public class AuthServiceBddTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly AuthService _authService;

        public AuthServiceBddTests()
        {
            _repositoryMock = new Mock<IUserRepository>();

            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["Jwt:Key"]).Returns("chave_super_secreta_para_testes_com_32chars!!");
            configMock.Setup(c => c["Jwt:Issuer"]).Returns("FCG.Api.Tests");
            configMock.Setup(c => c["Jwt:Audience"]).Returns("FCG.Api.Tests");

            _authService = new AuthService(_repositoryMock.Object, configMock.Object);
        }

        [Fact]
        public async Task Dado_credenciais_validas_Quando_fazer_login_Entao_deve_retornar_token_jwt()
        {
            // Dado
            var senhaOriginal = "Senha@123";
            var usuario = new User("Juliana", new Email("juliana@email.com"), PasswordHasher.Hash(senhaOriginal), UserRole.User);

            _repositoryMock
                .Setup(r => r.GetByEmailAsync("juliana@email.com"))
                .ReturnsAsync(usuario);

            var request = new LoginRequest
            {
                Email = "juliana@email.com",
                Password = senhaOriginal
            };

            // Quando
            var token = await _authService.LoginAsync(request);

            // Então
            token.Should().NotBeNullOrEmpty();
            token.Split('.').Should().HaveCount(3, "um JWT válido possui 3 partes separadas por ponto");
        }

        [Fact]
        public async Task Dado_usuario_nao_cadastrado_Quando_fazer_login_Entao_deve_lancar_excecao_de_acesso_negado()
        {
            // Dado
            _repositoryMock
                .Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            var request = new LoginRequest
            {
                Email = "naoexiste@email.com",
                Password = "Senha@123"
            };

            // Quando
            var acao = async () => await _authService.LoginAsync(request);

            // Então
            await acao.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Credenciais inválidas");
        }

        [Fact]
        public async Task Dado_senha_incorreta_Quando_fazer_login_Entao_deve_lancar_excecao_de_acesso_negado()
        {
            // Dado
            var usuario = new User("Juliana", new Email("juliana@email.com"), PasswordHasher.Hash("Senha@123"), UserRole.User);

            _repositoryMock
                .Setup(r => r.GetByEmailAsync("juliana@email.com"))
                .ReturnsAsync(usuario);

            var request = new LoginRequest
            {
                Email = "juliana@email.com",
                Password = "SenhaErrada@999"
            };

            // Quando
            var acao = async () => await _authService.LoginAsync(request);

            // Então
            await acao.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Credenciais inválidas");
        }

        [Fact]
        public async Task Dado_admin_com_credenciais_validas_Quando_fazer_login_Entao_token_deve_conter_role_admin()
        {
            // Dado
            var senhaOriginal = "Admin@123";
            var admin = new User("Admin", new Email("admin@fcg.com"), PasswordHasher.Hash(senhaOriginal), UserRole.Admin);

            _repositoryMock
                .Setup(r => r.GetByEmailAsync("admin@fcg.com"))
                .ReturnsAsync(admin);

            var request = new LoginRequest
            {
                Email = "admin@fcg.com",
                Password = senhaOriginal
            };

            // Quando
            var token = await _authService.LoginAsync(request);

            // Então
            token.Should().NotBeNullOrEmpty();

            var payload = token.Split('.')[1];
            var paddedPayload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
            var json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(paddedPayload));
            json.Should().Contain("Admin");
        }
    }
}
