namespace FCG.Api.Domain.ValueObjects
{
    public class Email
    {
        public string Value { get; private set; }

        protected Email()
        {
            Value = null!;
        }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                throw new ArgumentException("Email inválido");

            Value = value.ToLower();
        }

        public override string ToString() => Value;
    }
}
