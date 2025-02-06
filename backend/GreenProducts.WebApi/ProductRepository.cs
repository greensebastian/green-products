using GreenProducts.Core;
using GreenProducts.Core.Products;
using Microsoft.EntityFrameworkCore;

namespace GreenProducts.WebApi;

public class ProductRepository(GreenProductsDbContext context) : IProductRepository
{
    public async Task<PaginatedResponse<Product>> GetProducts(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var totalCount = await context.Products.CountAsync(cancellationToken);
        var resultItems = await context.Products
            .OrderBy(product => product.CreatedOn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        return new PaginatedResponse<Product>
        {
            Items = resultItems,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<Product?> GetProductDetails(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Product> AddProduct(Product product, CancellationToken cancellationToken = default)
    {
        context.Products.Add(product);
        return Task.FromResult(product);
    }
    
    public async Task<PaginatedResponse<ProductClassification>> GetProductClassifications(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var totalCount = await context.ProductClassifications.CountAsync(cancellationToken);
        var resultItems = await context.ProductClassifications
            .OrderBy(productClassification => productClassification.CreatedOn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        return new PaginatedResponse<ProductClassification>
        {
            Items = resultItems,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<IEnumerable<ProductClassification>> GetProductClassificationDetails(ICollection<Guid> ids, CancellationToken cancellationToken = default)
    {
        return await context.ProductClassifications.Where(productClassification => ids.Contains(productClassification.Id)).ToListAsync(cancellationToken);
    }

    public Task<ProductClassification> AddProductClassification(ProductClassification productClassification,
        CancellationToken cancellationToken = default)
    {
        context.ProductClassifications.Add(productClassification);
        return Task.FromResult(productClassification);
    }

    public async Task Save(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}