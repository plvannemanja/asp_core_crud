using InitProject.Models;
using Microsoft.EntityFrameworkCore;

namespace InitProject.Models;

public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .Property(product => product.ProductId)
            .ValueGeneratedOnAdd();
    }
}