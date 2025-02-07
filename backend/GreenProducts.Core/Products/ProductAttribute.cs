namespace GreenProducts.Core.Products;

/// <summary>
/// An attribute is a value with a display name that falls within some specific grouping/category.
/// Examples are:
/// Type: Colour, Value: GREEN, DisplayName: Green
/// Type: ProductType, Value: CHAIR, DisplayName: Chair
/// </summary>
public class ProductAttribute
{
    public required Guid Id { get; init; }
    public required string Type { get; set; }
    public required string Value { get; set; }
    public required string DisplayName { get; set; }
    public required DateTimeOffset CreatedOn { get; init; }

    public static class Types
    {
        public const string Colour = "COLOUR";
        public const string ProductType = "PRODUCT_TYPE";
    }
}