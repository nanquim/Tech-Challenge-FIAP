# FIAP Cloud Games – Tech Challenge Fase 1

API desenvolvida em **.NET 8** para o Tech Challenge (Fase 1), com foco em cadastro de usuários, autenticação, gestão de jogos e boas práticas de arquitetura.

---

## Tecnologias
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- JWT (Autenticação)
- xUnit + FluentAssertions + Moq (Testes)

---

## Arquitetura

O projeto segue os princípios de **Clean Architecture / DDD**, com cada camada isolada em seu próprio projeto:

```
FiapCloudGames/
├── FGC.Domain/          # Entidades, Value Objects, Enums, Interfaces de Repositório
├── FGC.Application/     # Casos de uso, DTOs, Validadores, Serviços de aplicação
├── FGC.Infrastructure/  # Repositórios, DbContext, Migrations, Middlewares
└── FGC.Api/             # Controllers, Program.cs (ponto de entrada)
```

**Regra de dependência:** Domain não depende de nada. Application depende de Domain. Infrastructure depende de Domain e Application. Api depende de Application e Infrastructure.

---

## Middlewares

Dois middlewares customizados estão implementados em `FGC.Infrastructure/Middlewares/` e registrados no pipeline do `Program.cs`:

| Middleware | Responsabilidade |
|---|---|
| `ExceptionHandlingMiddleware` | Captura exceções globais e retorna respostas HTTP padronizadas em JSON (`400`, `401`, `500`) |
| `RequestLoggingMiddleware` | Registra método e caminho de todas as requisições recebidas |

Registro no pipeline (`Program.cs`):
```csharp
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
```

---

## Endpoints

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| POST | `/users` | Não | Cadastrar novo usuário |
| POST | `/auth/login` | Não | Autenticar e obter token JWT |
| POST | `/games` | Admin | Cadastrar novo jogo |
| GET | `/games` | User / Admin | Listar todos os jogos |

---

## Executar o projeto

Na raiz da solução:

```bash
dotnet restore
dotnet run --project FGC.Api
```

Acesse o Swagger em: `http://localhost:<porta>/swagger`

---

## Executar os testes

```bash
dotnet test
```

Os testes seguem o padrão **BDD** (Dado / Quando / Então) e cobrem:

- `EmailValidator` — validação de formato de e-mail
- `PasswordValidator` — validação de regras de senha
- `UserService` — criação de usuário (sucesso, e-mail inválido, senha inválida, e-mail duplicado, hash de senha)
- `AuthService` — login (sucesso, usuário não encontrado, senha incorreta, role no token)
- `GameService` — criação e listagem de jogos
