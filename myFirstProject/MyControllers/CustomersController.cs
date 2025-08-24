using Microsoft.AspNetCore.Mvc;
using myFirstProject.Models;
using myFirstProject.MyModels;
using myFirstProject.Repository;

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _repository;
    private const int MaxRecords = 5; // default max

    public CustomersController(ICustomerRepository repository)
    {
        _repository = repository;
    }

    // GET /Customers
    [HttpGet]
    public IEnumerable<CustomerViewModel> Get([FromQuery] string name = null, [FromQuery] int? customerId = null, [FromQuery] int maxNumberOfRecords = MaxRecords)
    {
        maxNumberOfRecords = Math.Min(maxNumberOfRecords, MaxRecords); // enforce max

        IEnumerable<Customer> customers;

        if (customerId.HasValue)
        {
            var customer = _repository.GetById(customerId.Value);
            customers = customer != null ? new[] { customer } : Enumerable.Empty<Customer>();
        }
        else if (!string.IsNullOrEmpty(name))
        {
            customers = _repository.QueryByName(name, maxNumberOfRecords);
        }
        else
        {
            customers = _repository.GetCustomers(maxNumberOfRecords);
        }

        // Convert Customer to CustomerViewModel to get those sweet computed properties! 🎯
        return customers.Select(ConvertToViewModel);
    }

    /// <summary>
    /// Converts a Customer entity to CustomerViewModel with computed properties
    /// </summary>
    private static CustomerViewModel ConvertToViewModel(Customer customer)
    {
        return new CustomerViewModel
        {
            CustomerID = customer.CustomerID,
            NameStyle = customer.NameStyle,
            Title = customer.Title,
            FirstName = customer.FirstName,
            MiddleName = customer.MiddleName,
            LastName = customer.LastName,
            Suffix = customer.Suffix,
            CompanyName = customer.CompanyName,
            SalesPerson = customer.SalesPerson,
            EmailAddress = customer.EmailAddress,
            Phone = customer.Phone,
            PasswordHash = customer.PasswordHash,
            PasswordSalt = customer.PasswordSalt,
            rowguid = customer.rowguid,
            ModifiedDate = customer.ModifiedDate
            // FullName, DisplayName, and Initials are automatically computed! ✨
        };
    }
}

