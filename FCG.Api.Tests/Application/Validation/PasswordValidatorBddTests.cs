using Xunit;
using FluentAssertions;
using FCG.Api.Application.Validators;

namespace FCG.Api.Tests.Application.Validation
{
    public class ValidadorSenhaBDDTests
    {
        [Fact]
        public void Dado_senha_com_menos_de_8_caracteres_Quando_validar_Entao_deve_retornar_falso()
        {
            // Dado
            var senha = "Abc1!";

            // Quando
            var resultado = PasswordValidator.IsValid(senha);

            // Então
            resultado.Should().BeFalse();
        }

        [Fact]
        public void Dado_senha_sem_numero_Quando_validar_Entao_deve_retornar_falso()
        {
            // Dado
            var senha = "Abcdef!@";

            // Quando
            var resultado = PasswordValidator.IsValid(senha);

            // Então
            resultado.Should().BeFalse();
        }

        [Fact]
        public void Dado_senha_sem_caractere_especial_Quando_validar_Entao_deve_retornar_falso()
        {
            // Dado
            var senha = "Abcdef12";

            // Quando
            var resultado = PasswordValidator.IsValid(senha);

            // Então
            resultado.Should().BeFalse();
        }

        [Fact]
        public void Dado_senha_valida_Quando_validar_Entao_deve_retornar_verdadeiro()
        {
            // Dado
            var senha = "Abcd123!";

            // Quando
            var resultado = PasswordValidator.IsValid(senha);

            // Então
            resultado.Should().BeTrue();
        }
    }
}
