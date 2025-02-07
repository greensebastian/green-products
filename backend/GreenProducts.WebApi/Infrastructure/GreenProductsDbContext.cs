using GreenProducts.Core.Products;
using Microsoft.EntityFrameworkCore;

namespace GreenProducts.WebApi.Infrastructure;

public class GreenProductsDbContext(DbContextOptions<GreenProductsDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductClassification> ProductClassifications { get; set; }

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

        var productClassificationModel = modelBuilder.Entity<ProductClassification>();
        productClassificationModel.HasKey(productClassification => productClassification.Id);
        productClassificationModel.HasIndex(productClassification => productClassification.Type);
        productClassificationModel.HasIndex(productClassification => new { productClassification.Type, productClassification.Value });
        productClassificationModel.HasIndex(productClassification => productClassification.CreatedOn);
    }
}