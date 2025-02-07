namespace GreenProducts.Core.Products;

// I opted to do exception based flow for not found and validation errors because of simplicity.
// Would do result-pattern for more critical applications.

public class ProductNotFoundException(string message) : ApplicationException(message);

public class ProductValidationException(string message) : ApplicationException(message);