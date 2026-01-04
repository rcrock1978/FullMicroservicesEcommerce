using CartService.Domain.Entities;
using CartService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CartService.Infrastructure.Data;

public class CartDbContext : DbContext
{
    public CartDbContext(DbContextOptions<CartDbContext> options) : base(options)
    {
    }

    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CartDbContext).Assembly);
    }
}
