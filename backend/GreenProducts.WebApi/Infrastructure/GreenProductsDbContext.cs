using GreenProducts.Core.Products;
using Microsoft.EntityFrameworkCore;

namespace GreenProducts.WebApi.Infrastructure;

public class GreenProductsDbContext(DbContextOptions<GreenProductsDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductAttribute> ProductAttributes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var productModel = modelBuilder.Entity<Product>();
        productModel.HasKey(product => product.Id);
        productModel.HasIndex(product => product.CreatedOn);
        productModel
            .HasOne(product => product.ProductType)
            .WithMany();
        productModel
            .HasMany(product => product.AvailableColours)
            .WithMany();
        productModel.Navigation(product => product.ProductType).AutoInclude();
        productModel.Navigation(product => product.AvailableColours).AutoInclude();

        var productAttributeModel = modelBuilder.Entity<ProductAttribute>();
        productAttributeModel.HasKey(productAttribute => productAttribute.Id);
        productAttributeModel.HasIndex(productAttribute => productAttribute.Type);
        productAttributeModel.HasIndex(productAttribute => new { productAttribute.Type, productAttribute.Value });
        productAttributeModel.HasIndex(productAttribute => productAttribute.CreatedOn);
    }
}