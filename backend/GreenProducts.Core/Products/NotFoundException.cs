namespace GreenProducts.Core.Products;

public class NotFoundException : ApplicationException
{
    public required Guid Id { get; init; }
}