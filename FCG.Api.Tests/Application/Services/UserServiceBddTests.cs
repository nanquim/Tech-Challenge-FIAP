using FluentAssertions;
using Moq;
using FCG.Api.Application.DTOs;
using FCG.Api.Application.Services;
using FCG.Api.Domain.Entities;
using FCG.Api.Domain.Repositories;

namespace FCG.Api.Tests.Application.Services
{
    public class UserServiceBddTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly UserService _userService;

        public UserServiceBddTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_repositoryMock.Object);
        }

        [Fact]
        public async Task Dado_requisicao_valida_Quando_criar_usuario_Entao_deve_retornar_id_do_usuario()
        {
            // Dado
            var request = new CreateUserRequest
            {
                Name = "Juliana",
                Email = "juliana@email.com",
                Password = "Senha@123"
            };

            _repositoryMock
                .Setup(r => r.GetByEmailAsync(request.Email))
                .ReturnsAsync((User?)null);

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            // Quando
            var resultado = await _userService.CreateAsync(request);

            // Então
            resultado.Should().NotBeEmpty();
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Dado_email_invalido_Quando_criar_usuario_Entao_deve_lancar_excecao_de_argumento()
        {
            // Dado
            var request = new CreateUserRequest
            {
                Name = "Juliana",
                Email = "email-invalido",
                Password = "Senha@123"
            };

            // Quando
            var acao = async () => await _userService.CreateAsync(request);

            // Então
            await acao.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Email inválido");
        }

        [Fact]
        public async Task Dado_senha_invalida_Quando_criar_usuario_Entao_deve_lancar_excecao_de_argumento()
        {
            // Dado
            var request = new CreateUserRequest
            {
                Name = "Juliana",
                Email = "juliana@email.com",
                Password = "fraca"
            };

            // Quando
            var acao = async () => await _userService.CreateAsync(request);

            // Então
            await acao.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Senha inválida");
        }

        [Fact]
        public async Task Dado_email_ja_cadastrado_Quando_criar_usuario_Entao_deve_lancar_excecao_de_argumento()
        {
            // Dado
            var request = new CreateUserRequest
            {
                Name = "Juliana",
                Email = "juliana@email.com",
                Password = "Senha@123"
            };

            var usuarioExistente = new User("Outro", request.Email, "hash", FCG.Api.Domain.Enums.UserRole.User);

            _repositoryMock
                .Setup(r => r.GetByEmailAsync(request.Email))
                .ReturnsAsync(usuarioExistente);

            // Quando
            var acao = async () => await _userService.CreateAsync(request);

            // Então
            await acao.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Email já cadastrado");
        }

        [Fact]
        public async Task Dado_requisicao_valida_Quando_criar_usuario_Entao_nao_deve_salvar_senha_em_texto_puro()
        {
            // Dado
            var senhaOriginal = "Senha@123";
            var request = new CreateUserRequest
            {
                Name = "Juliana",
                Email = "juliana@email.com",
                Password = senhaOriginal
            };

            User? usuarioSalvo = null;

            _repositoryMock
                .Setup(r => r.GetByEmailAsync(request.Email))
                .ReturnsAsync((User?)null);

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<User>()))
                .Callback<User>(u => usuarioSalvo = u)
                .Returns(Task.CompletedTask);

            // Quando
            await _userService.CreateAsync(request);

            // Então
            usuarioSalvo.Should().NotBeNull();
            usuarioSalvo!.PasswordHash.Should().NotBe(senhaOriginal);
        }
    }
}
