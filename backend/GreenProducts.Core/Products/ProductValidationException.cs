namespace GreenProducts.Core.Products;

public class ProductValidationException : ApplicationException
{
    public required string[] Reasons { get; init; }
}