namespace GreenProducts.Core;

/// <summary>
/// Paginated response containing a collection of items as well as query data.
/// </summary>
/// <typeparam name="T">Type of items returned</typeparam>
public class PaginatedResponse<T> where T : class
{
    public required List<T> Items { get; init; }
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public required int TotalCount { get; init; }
}