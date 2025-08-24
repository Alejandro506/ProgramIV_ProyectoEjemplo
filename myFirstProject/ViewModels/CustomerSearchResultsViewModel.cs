using myFirstProject.MyModels;

namespace myFirstProject.ViewModels;

/// <summary>
/// View model for customer search results
/// </summary>
public class CustomerSearchResultsViewModel
{
    public CustomerSearchViewModel SearchCriteria { get; set; } = new();
    public List<CustomerViewModel> Results { get; set; } = new();
    public string? ErrorMessage { get; set; }
    public bool HasResults => Results.Any();
    public int ResultCount => Results.Count;
}
