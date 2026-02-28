using FluentAssertions;
using Moq;
using FCG.Api.Application.DTOs;
using FCG.Api.Application.Services;
using FCG.Api.Domain.Entities;
using FCG.Api.Domain.Repositories;

namespace FCG.Api.Tests.Application.Services
{
    public class GameServiceBddTests
    {
        private readonly Mock<IGameRepository> _repositoryMock;
        private readonly GameService _gameService;

        public GameServiceBddTests()
        {
            _repositoryMock = new Mock<IGameRepository>();
            _gameService = new GameService(_repositoryMock.Object);
        }

        [Fact]
        public async Task Dado_dados_validos_Quando_criar_jogo_Entao_deve_retornar_id_do_jogo()
        {
            // Dado
            var request = new CreateGameRequest
            {
                Title = "The Last of Us",
                Description = "Jogo de ação e aventura pós-apocalíptico",
                Price = 199.90m
            };

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Game>()))
                .Returns(Task.CompletedTask);

            // Quando
            var resultado = await _gameService.CreateAsync(request);

            // Então
            resultado.Should().NotBeEmpty();
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Game>()), Times.Once);
        }

        [Fact]
        public async Task Dado_jogos_cadastrados_Quando_buscar_todos_Entao_deve_retornar_lista_completa()
        {
            // Dado
            var jogos = new List<Game>
            {
                new Game("The Last of Us", "Aventura pós-apocalíptica", 199.90m),
                new Game("God of War", "Ação e mitologia nórdica", 249.90m),
                new Game("Horizon Zero Dawn", "RPG de mundo aberto", 149.90m)
            };

            _repositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(jogos);

            // Quando
            var resultado = await _gameService.GetAllAsync();

            // Então
            resultado.Should().HaveCount(3);
            resultado.Should().Contain(j => j.Title == "The Last of Us");
        }

        [Fact]
        public async Task Dado_nenhum_jogo_cadastrado_Quando_buscar_todos_Entao_deve_retornar_lista_vazia()
        {
            // Dado
            _repositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Game>());

            // Quando
            var resultado = await _gameService.GetAllAsync();

            // Então
            resultado.Should().BeEmpty();
        }

        [Fact]
        public async Task Dado_titulo_vazio_Quando_criar_jogo_Entao_deve_lancar_excecao_de_argumento()
        {
            // Dado
            var request = new CreateGameRequest
            {
                Title = "",
                Description = "Descrição válida",
                Price = 50m
            };

            // Quando
            var acao = async () => await _gameService.CreateAsync(request);

            // Então
            await acao.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Título é obrigatório");
        }

        [Fact]
        public async Task Dado_preco_negativo_Quando_criar_jogo_Entao_deve_lancar_excecao_de_argumento()
        {
            // Dado
            var request = new CreateGameRequest
            {
                Title = "Jogo Válido",
                Description = "Descrição válida",
                Price = -10m
            };

            // Quando
            var acao = async () => await _gameService.CreateAsync(request);

            // Então
            await acao.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Preço não pode ser negativo");
        }

        [Fact]
        public async Task Dado_dados_validos_Quando_criar_jogo_Entao_jogo_deve_ter_id_unico()
        {
            // Dado
            var request1 = new CreateGameRequest { Title = "Jogo A", Description = "Desc A", Price = 50m };
            var request2 = new CreateGameRequest { Title = "Jogo B", Description = "Desc B", Price = 80m };

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Game>()))
                .Returns(Task.CompletedTask);

            // Quando
            var id1 = await _gameService.CreateAsync(request1);
            var id2 = await _gameService.CreateAsync(request2);

            // Então
            id1.Should().NotBe(id2);
        }
    }
}
