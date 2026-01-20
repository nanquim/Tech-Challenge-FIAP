using System.Text.RegularExpressions;

namespace FCG.Api.Application.Validators
{
    public static class EmailValidator
    {
        public static bool IsValid(string email)
        {
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
