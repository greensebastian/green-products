namespace GreenProducts.Core.Products;

/// <summary>
/// A classification is a value with a display name that falls within some specific grouping/category.
/// Examples are:
/// Type: Colour, Value: GREEN, DisplayName: Green
/// Type: ProductType, Value: CHAIR, DisplayName: Chair
/// </summary>
public class ProductClassification
{
    public required Guid Id { get; init; }
    public required string Type { get; set; }
    public required string Value { get; set; }
    public required string DisplayName { get; set; }
    
    public bool IsColour => Type == "COLOUR";
    public bool IsProductType => Type == "PRODUCT_TYPE";
}