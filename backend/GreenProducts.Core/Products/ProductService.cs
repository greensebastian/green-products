namespace GreenProducts.Core.Products;

public class ProductService(IProductRepository productRepository, TimeProvider timeProvider)
{
    public async Task<PaginatedResponse<Product>> GetProducts(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return await productRepository.GetProducts(page, pageSize, cancellationToken);
    }

    public async Task<Product> GetProductDetails(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetProductDetails(id, cancellationToken);
        return product ?? throw new KeyNotFoundException($"Product with id {id} not found");
    }

    public async Task<Product> AddProduct(string name, Guid productTypeId, ICollection<Guid> colourIds, CancellationToken cancellationToken = default)
    {
        var colourClassifications = await GetProductClassificationDetails(ProductClassification.Types.Colour, colourIds, cancellationToken);
        var productTypeClassification = (await GetProductClassificationDetails(ProductClassification.Types.ProductType, [productTypeId], cancellationToken)).Single();
        
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            ProductType = productTypeClassification,
            AvailableColours = colourClassifications.ToList(),
            CreatedOn = timeProvider.GetUtcNow()
        };

        var created = await productRepository.AddProduct(product, cancellationToken);
        await productRepository.Save(cancellationToken);
        return created;
    }

    public async Task<PaginatedResponse<ProductClassification>> GetProductClassifications(int page = 1, int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        return await productRepository.GetProductClassifications(page, pageSize, cancellationToken);
    }
    
    private async Task<IEnumerable<ProductClassification>> GetProductClassificationDetails(string desiredType, ICollection<Guid> classificationIds, CancellationToken cancellationToken = default)
    {
        var classifications = await productRepository.GetProductClassificationDetails(classificationIds, cancellationToken);
        
        var invalidClassifications = classificationIds
            .Select(classificationId => new { Id = classificationId, Classification = classifications.SingleOrDefault(classification => classification.Id == classificationId)})
            .Where(match => match.Classification is null || match.Classification.Type != desiredType).ToArray();
        if (invalidClassifications.Any())
        {
            throw new ArgumentOutOfRangeException(
                $"Classifications {string.Join(", ", invalidClassifications.Select(c => c.Id))} do not exist or do not match type {desiredType}");
        }
        
        return classifications;
    }
}