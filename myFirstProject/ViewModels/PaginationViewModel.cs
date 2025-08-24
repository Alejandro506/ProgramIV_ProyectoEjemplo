using myFirstProject.MyModels;

namespace myFirstProject.ViewModels;

/// <summary>
/// View model for pagination that includes search criteria
/// </summary>
public class PaginationViewModel
{
    /// <summary>
    /// Pagination metadata
    /// </summary>
    public PaginationModel Pagination { get; set; } = new();
    
    /// <summary>
    /// Search criteria to preserve in pagination links
    /// </summary>
    public string? Name { get; set; }
}
