using myFirstProject.Models;

namespace myFirstProject.MyModels;

/// <summary>
/// Enhanced Customer model with computed properties for better API responses
/// </summary>
public class CustomerViewModel : Customer
{
    /// <summary>
    /// Gets the full name by combining FirstName, MiddleName, and LastName
    /// </summary>
    public string FullName
    {
        get
        {
            var parts = new[] { FirstName, MiddleName, LastName }
                .Where(part => !string.IsNullOrWhiteSpace(part));
            return string.Join(" ", parts);
        }
    }

    /// <summary>
    /// Gets the display name including title and suffix if available
    /// </summary>
    public string DisplayName
    {
        get
        {
            var parts = new[] { Title, FullName, Suffix }
                .Where(part => !string.IsNullOrWhiteSpace(part));
            return string.Join(" ", parts);
        }
    }

    /// <summary>
    /// Gets the initials from the name
    /// </summary>
    public string Initials
    {
        get
        {
            var initials = string.Empty;
            if (!string.IsNullOrWhiteSpace(FirstName)) initials += FirstName[0];
            if (!string.IsNullOrWhiteSpace(MiddleName)) initials += MiddleName[0];
            if (!string.IsNullOrWhiteSpace(LastName)) initials += LastName[0];
            return initials.ToUpperInvariant();
        }
    }
}

