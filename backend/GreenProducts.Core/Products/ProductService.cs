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
    /// <param name="productTypeId">Id of attribute corresponding to product type</param>
    /// <param name="colourIds">Ids of attributes corresponding to available colours for this product</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created product</returns>
    public async Task<Product> AddProduct(string name, Guid productTypeId, ICollection<Guid> colourIds, CancellationToken cancellationToken = default)
    {
        if (await productRepository.ProductExists(name, cancellationToken))
            throw new ProductValidationException($"Product with name {name} already exists");
        
        var colourAttributes = await GetNotEmptyProductAttributeDetails(ProductAttribute.Types.Colour, colourIds, cancellationToken);
        var productTypeAttribute = (await GetNotEmptyProductAttributeDetails(ProductAttribute.Types.ProductType, [productTypeId], cancellationToken)).Single();

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            ProductType = productTypeAttribute,
            AvailableColours = colourAttributes.ToList(),
            CreatedOn = timeProvider.GetUtcNow().Truncate(TimeSpan.TicksPerMicrosecond)
        };
        
        var created = await productRepository.AddProduct(product, cancellationToken);
        await productRepository.Save(cancellationToken);
        return created;
    }

    /// <summary>
    /// Get paginated list of product attributes in the catalogue.
    /// </summary>
    /// <param name="page">Page to fetch, index starts at 1</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of product attributes in the catalogue</returns>
    public async Task<PaginatedResponse<ProductAttribute>> GetProductAttributes(int page = 1, int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        return await productRepository.GetProductAttributes(page, pageSize, cancellationToken);
    }

    /// <summary>
    /// Helper method to fetch and validate attributes against desired type.
    /// </summary>
    /// <returns>Collection of fetched attributes</returns>
    /// <exception cref="ProductValidationException">Exception if requested ids don't exist, or they are of incorrect type</exception>
    private async Task<ICollection<ProductAttribute>> GetNotEmptyProductAttributeDetails(string desiredType, ICollection<Guid> attributeIds, CancellationToken cancellationToken = default)
    {
        var attributes = (await productRepository.GetProductAttributeDetails(attributeIds, cancellationToken)).ToArray();

        if (!attributes.Any())
        {
            throw new ProductValidationException($"Query for attribute [{string.Join(", ", attributeIds)}] of type {desiredType} returned no attributes");
        }

        var invalidAttributes = attributeIds
            .Select(attributeId => new { Id = attributeId, Attribute = attributes.SingleOrDefault(attribute => attribute.Id == attributeId) })
            .Where(match => match.Attribute is null || match.Attribute.Type != desiredType).ToArray();
        if (invalidAttributes.Any())
        {
            throw new ProductValidationException($"Attributes [{string.Join(", ", invalidAttributes.Select(c => c.Id))}] do not exist or do not match type {desiredType}");
        }

        return attributes;
    }
}