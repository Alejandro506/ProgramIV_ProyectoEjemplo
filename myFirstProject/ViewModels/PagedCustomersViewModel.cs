using myFirstProject.MyModels;

namespace myFirstProject.ViewModels;

/// <summary>
/// View model for paginated customers search
/// </summary>
public class PagedCustomersViewModel
{
    /// <summary>
    /// Current page number (1-based)
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Number of items per page
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Name to search for (optional)
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The paginated results
    /// </summary>
    public PaginatedResult<CustomerViewModel>? Results { get; set; }

    /// <summary>
    /// Error message if any
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets available page sizes
    /// </summary>
    public static List<int> AvailablePageSizes => new List<int> { 5, 10, 20, 50 };
}
