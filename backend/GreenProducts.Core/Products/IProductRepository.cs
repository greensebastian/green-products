namespace GreenProducts.Core.Products;

public interface IProductRepository
{
    public Task<PaginatedResponse<Product>> GetProducts(int page, int pageSize, CancellationToken cancellationToken = default);

    public Task<Product?> GetProductDetails(Guid id, CancellationToken cancellationToken = default);

    public Task<Product> AddProduct(Product product, CancellationToken cancellationToken = default);

    public Task<PaginatedResponse<ProductClassification>> GetProductClassifications(int page, int pageSize, CancellationToken cancellationToken = default);

    public Task<IEnumerable<ProductClassification>> GetProductClassificationDetails(ICollection<Guid> ids, CancellationToken cancellationToken = default);

    public Task<ProductClassification> AddProductClassification(ProductClassification productClassification, CancellationToken cancellationToken = default);

    public Task Save(CancellationToken cancellationToken = default);
}