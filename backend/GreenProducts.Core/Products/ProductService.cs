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
        return product ?? throw new NotFoundException{ Id = id };
    }

    public async Task<Product> AddProduct(string name, Guid productTypeId, ICollection<Guid> colourIds, CancellationToken cancellationToken = default)
    {
        var relevantClassifications = (await productRepository.GetProductClassifications(colourIds.Distinct().Prepend(productTypeId), cancellationToken)).ToArray();
        
        // Ensure colour classification is valid
        var desiredColourClassifications = relevantClassifications.Where(c => colourIds.Contains(c.Id)).ToList();
        var invalidColours = desiredColourClassifications.Where(c => !c.IsColour).ToArray();
        if (invalidColours.Any())
        {
            throw new ProductValidationException
            {
                Reasons = invalidColours.Select(c => $"Classification {c.Id} [{c.Value}] is not a valid colour classification").ToArray()
            };
        }
        
        // Ensure product type classification is valid
        var desiredProductTypeClassification = relevantClassifications.SingleOrDefault(c => c.Id == productTypeId);
        if (desiredProductTypeClassification is null || !desiredProductTypeClassification.IsProductType)
        {
            throw new ProductValidationException
            {
                Reasons = [$"Classification {productTypeId} [{desiredProductTypeClassification?.Value}] is not a valid product type classification"]
            };
        }
        
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            ProductType = desiredProductTypeClassification,
            AvailableColours = desiredColourClassifications,
            CreatedOn = timeProvider.GetUtcNow()
        };

        return await productRepository.AddProduct(product, cancellationToken);
    }
}