namespace GreenProducts.Core.Products;

/// <summary>
/// A product refers to one specific product available, such as a model of a chair or sofa, with available colours.
/// </summary>
public class Product
{
    public required Guid Id { get; init; }
    public required string Name { get; set; }
    public required ProductClassification ProductType { get; set; }
    public required List<ProductClassification> AvailableColours { get; set; }
    public required DateTimeOffset CreatedOn { get; init; }
}