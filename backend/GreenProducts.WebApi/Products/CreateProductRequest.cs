namespace GreenProducts.WebApi.Products;

public record CreateProductRequest(string Name, Guid ProductTypeId, ICollection<Guid> ColourIds);