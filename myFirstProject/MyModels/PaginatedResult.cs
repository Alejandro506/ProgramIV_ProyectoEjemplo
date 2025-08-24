namespace myFirstProject.MyModels;

/// <summary>
/// Generic wrapper for paginated results
/// </summary>
/// <typeparam name="T">The type of items in the result</typeparam>
public class PaginatedResult<T>
{
    /// <summary>
    /// The items for the current page
    /// </summary>
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

    /// <summary>
    /// Pagination metadata
    /// </summary>
    public PaginationModel Pagination { get; set; } = new();

    /// <summary>
    /// Creates a new paginated result
    /// </summary>
    /// <param name="items">The items for the current page</param>
    /// <param name="totalItems">Total number of items across all pages</param>
    /// <param name="pageNumber">Current page number</param>
    /// <param name="pageSize">Number of items per page</param>
    public static PaginatedResult<T> Create(IEnumerable<T> items, int totalItems, int pageNumber, int pageSize)
    {
        return new PaginatedResult<T>
        {
            Items = items,
            Pagination = new PaginationModel
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems
            }
        };
    }
}
