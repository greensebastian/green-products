namespace GreenProducts.Core.Products;

public interface IProductRepository
{
    public Task<PaginatedResponse<Product>> GetProducts(int page, int pageSize, CancellationToken cancellationToken = default);

    public Task<Product?> GetProductDetails(Guid id, CancellationToken cancellationToken = default);

    public Task<Product> AddProduct(Product product, CancellationToken cancellationToken = default);

    public Task<PaginatedResponse<ProductAttribute>> GetProductAttributes(int page, int pageSize, CancellationToken cancellationToken = default);

    public Task<IEnumerable<ProductAttribute>> GetProductAttributeDetails(ICollection<Guid> ids, CancellationToken cancellationToken = default);

    public Task Save(CancellationToken cancellationToken = default);
}