using GreenProducts.Core;
using GreenProducts.Core.Products;
using Microsoft.EntityFrameworkCore;

namespace GreenProducts.WebApi.Infrastructure;

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

    public async Task<PaginatedResponse<ProductAttribute>> GetProductAttributes(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var totalCount = await context.ProductAttributes.CountAsync(cancellationToken);
        var resultItems = await context.ProductAttributes
            .OrderBy(productAttribute => productAttribute.CreatedOn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResponse<ProductAttribute>
        {
            Items = resultItems,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<IEnumerable<ProductAttribute>> GetProductAttributeDetails(ICollection<Guid> ids, CancellationToken cancellationToken = default)
    {
        return await context.ProductAttributes.Where(productAttribute => ids.Contains(productAttribute.Id)).ToListAsync(cancellationToken);
    }

    public async Task Save(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}