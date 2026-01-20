namespace FCG.Api.Domain.Entities
{
    public class Game
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }

        protected Game()
        {
            Title = null!;
            Description = null!;
        }


        public Game(string title, string description, decimal price)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Price = price;
        }
    }
}
