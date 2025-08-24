namespace myFirstProject.MyModels;

/// <summary>
/// Model for handling pagination parameters and metadata
/// </summary>
public class PaginationModel
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
    /// Total number of items across all pages
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

    /// <summary>
    /// Indicates if there is a previous page
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Indicates if there is a next page
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>
    /// Number of items to skip for the current page
    /// </summary>
    public int Skip => (PageNumber - 1) * PageSize;

    /// <summary>
    /// Gets a list of page numbers for pagination navigation
    /// </summary>
    public IEnumerable<int> GetPageNumbers(int maxPagesToShow = 10)
    {
        var start = Math.Max(1, PageNumber - maxPagesToShow / 2);
        var end = Math.Min(TotalPages, start + maxPagesToShow - 1);
        
        // Adjust start if we're near the end
        if (end - start + 1 < maxPagesToShow)
        {
            start = Math.Max(1, end - maxPagesToShow + 1);
        }

        return Enumerable.Range(start, end - start + 1);
    }
}
