using Microsoft.EntityFrameworkCore;
using FCG.Api.Domain.Entities;

namespace FCG.Api.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Game> Games => Set<Game>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Name).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Role).IsRequired();

                entity.HasIndex(u => u.Email).IsUnique();
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(g => g.Id);

                entity.Property(g => g.Title).IsRequired();
                entity.Property(g => g.Description).IsRequired();
                entity.Property(g => g.Price).IsRequired();
            });
        }
    }
}
