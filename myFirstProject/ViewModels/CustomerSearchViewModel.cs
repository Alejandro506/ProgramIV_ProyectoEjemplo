using System.ComponentModel.DataAnnotations;

namespace myFirstProject.ViewModels;

/// <summary>
/// View model for customer search criteria
/// </summary>
public class CustomerSearchViewModel
{
    [Display(Name = "Customer ID")]
    public int? CustomerID { get; set; }

    [Display(Name = "Name")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string? Name { get; set; }

    [Display(Name = "Max Number of Records")]
    [Range(1, 50, ErrorMessage = "Max number of records must be between 1 and 50")]
    public int MaxNumberOfRecords { get; set; } = 10;
}
