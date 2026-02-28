using FluentAssertions;
using FCG.Api.Application.Validators;

namespace FCG.Api.Tests.Application.Validation
{
    public class EmailValidatorBddTests
    {
        [Fact]
        public void Dado_email_valido_Quando_validar_Entao_deve_retornar_verdadeiro()
        {
            // Dado
            var email = "usuario@email.com";

            // Quando
            var resultado = EmailValidator.IsValid(email);

            // Então
            resultado.Should().BeTrue();
        }

        [Fact]
        public void Dado_email_sem_arroba_Quando_validar_Entao_deve_retornar_falso()
        {
            // Dado
            var email = "usuarioemail.com";

            // Quando
            var resultado = EmailValidator.IsValid(email);

            // Então
            resultado.Should().BeFalse();
        }

        [Fact]
        public void Dado_email_sem_dominio_Quando_validar_Entao_deve_retornar_falso()
        {
            // Dado
            var email = "usuario@";

            // Quando
            var resultado = EmailValidator.IsValid(email);

            // Então
            resultado.Should().BeFalse();
        }

        [Fact]
        public void Dado_email_sem_extensao_Quando_validar_Entao_deve_retornar_falso()
        {
            // Dado
            var email = "usuario@email";

            // Quando
            var resultado = EmailValidator.IsValid(email);

            // Então
            resultado.Should().BeFalse();
        }

        [Fact]
        public void Dado_email_vazio_Quando_validar_Entao_deve_retornar_falso()
        {
            // Dado
            var email = "";

            // Quando
            var resultado = EmailValidator.IsValid(email);

            // Então
            resultado.Should().BeFalse();
        }
    }
}
