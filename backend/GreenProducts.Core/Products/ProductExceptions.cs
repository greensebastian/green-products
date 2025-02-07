namespace GreenProducts.Core.Products;

public class ProductNotFoundException(string message) : ApplicationException(message);

public class ProductValidationException(string message) : ApplicationException(message);