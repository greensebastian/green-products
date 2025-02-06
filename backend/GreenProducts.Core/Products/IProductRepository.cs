namespace GreenProducts.Core.Products;

public interface IProductRepository
{
    public Task<PaginatedResponse<Product>> GetProducts(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    
    public Task<Product?> GetProductDetails(Guid id, CancellationToken cancellationToken = default);
    
    public Task<Product> AddProduct(Product product, CancellationToken cancellationToken = default);
    
    public Task<IEnumerable<ProductClassification>> GetProductClassifications(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    
    public Task<ProductClassification> AddProductClassification(ProductClassification productClassification, CancellationToken cancellationToken = default);

    public Task Save(CancellationToken cancellationToken = default);
}