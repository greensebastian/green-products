namespace GreenProducts.Core.Products;

public class ProductService(IProductRepository productRepository, TimeProvider timeProvider)
{
    /// <summary>
    /// Get paginated list of products in the catalogue.
    /// </summary>
    /// <param name="page">Page to fetch, index starts at 1</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of products in the catalogue</returns>
    public async Task<PaginatedResponse<Product>> GetProducts(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return await productRepository.GetProducts(page, pageSize, cancellationToken);
    }

    /// <summary>
    /// Gets details of a single product
    /// </summary>
    /// <param name="id">Id of product to fetch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Details of a single product</returns>
    /// <exception cref="ProductNotFoundException">Exception thrown if product doesn't exist</exception>
    public async Task<Product> GetProductDetails(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetProductDetails(id, cancellationToken);
        return product ?? throw new ProductNotFoundException($"Product with id {id} not found");
    }

    /// <summary>
    /// Adds the given product to the catalogue
    /// </summary>
    /// <param name="name">Name of product</param>
    /// <param name="productTypeId">Id of classification corresponding to product type</param>
    /// <param name="colourIds">Ids of classifications corresponding to available colours for this product</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created product</returns>
    public async Task<Product> AddProduct(string name, Guid productTypeId, ICollection<Guid> colourIds, CancellationToken cancellationToken = default)
    {
        var colourClassifications = await GetNotEmptyProductClassificationDetails(ProductClassification.Types.Colour, colourIds, cancellationToken);
        var productTypeClassification = (await GetNotEmptyProductClassificationDetails(ProductClassification.Types.ProductType, [productTypeId], cancellationToken)).Single();

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            ProductType = productTypeClassification,
            AvailableColours = colourClassifications.ToList(),
            CreatedOn = timeProvider.GetUtcNow().Truncate(TimeSpan.TicksPerMillisecond)
        };

        var created = await productRepository.AddProduct(product, cancellationToken);
        await productRepository.Save(cancellationToken);
        return created;
    }

    /// <summary>
    /// Get paginated list of product classifications in the catalogue.
    /// </summary>
    /// <param name="page">Page to fetch, index starts at 1</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of product classifications in the catalogue</returns>
    public async Task<PaginatedResponse<ProductClassification>> GetProductClassifications(int page = 1, int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        return await productRepository.GetProductClassifications(page, pageSize, cancellationToken);
    }

    /// <summary>
    /// Helper method to fetch and validate classifications against desired type.
    /// </summary>
    /// <returns>Collection of fetched classifications</returns>
    /// <exception cref="ProductValidationException">Exception if requested ids don't exist, or they are of incorrect type</exception>
    private async Task<ICollection<ProductClassification>> GetNotEmptyProductClassificationDetails(string desiredType, ICollection<Guid> classificationIds, CancellationToken cancellationToken = default)
    {
        var classifications = (await productRepository.GetProductClassificationDetails(classificationIds, cancellationToken)).ToArray();

        if (!classifications.Any())
        {
            throw new ProductValidationException($"Query for classifications [{string.Join(", ", classificationIds)}] of type {desiredType} returned no classifications");
        }

        var invalidClassifications = classificationIds
            .Select(classificationId => new { Id = classificationId, Classification = classifications.SingleOrDefault(classification => classification.Id == classificationId) })
            .Where(match => match.Classification is null || match.Classification.Type != desiredType).ToArray();
        if (invalidClassifications.Any())
        {
            throw new ProductValidationException($"Classifications [{string.Join(", ", invalidClassifications.Select(c => c.Id))}] do not exist or do not match type {desiredType}");
        }

        return classifications;
    }
}