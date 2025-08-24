using Microsoft.AspNetCore.Mvc;
using myFirstProject.Models;
using myFirstProject.MyModels;
using myFirstProject.Repository;

[ApiController]
[Route("[controller]")]
public class PagedCustomersController : ControllerBase
{
    private readonly ICustomerRepository _repository;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    public PagedCustomersController(ICustomerRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets customers with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <returns>Paginated list of customers</returns>
    [HttpGet]
    public ActionResult<PaginatedResult<CustomerViewModel>> Get(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = DefaultPageSize)
    {
        // Validate input parameters
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = DefaultPageSize;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        var result = _repository.GetCustomersPaginated(pageNumber, pageSize);
        
        // Convert Customer to CustomerViewModel
        var viewModelResult = new PaginatedResult<CustomerViewModel>
        {
            Items = result.Items.Select(ConvertToViewModel),
            Pagination = result.Pagination
        };

        return Ok(viewModelResult);
    }

    /// <summary>
    /// Searches customers by name with pagination
    /// </summary>
    /// <param name="name">Name to search for</param>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <returns>Paginated list of customers matching the search</returns>
    [HttpGet("search")]
    public ActionResult<PaginatedResult<CustomerViewModel>> Search(
        [FromQuery] string name,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = DefaultPageSize)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Name parameter is required for search");
        }

        // Validate input parameters
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = DefaultPageSize;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        var result = _repository.QueryByNamePaginated(name, pageNumber, pageSize);
        
        // Convert Customer to CustomerViewModel
        var viewModelResult = new PaginatedResult<CustomerViewModel>
        {
            Items = result.Items.Select(ConvertToViewModel),
            Pagination = result.Pagination
        };

        return Ok(viewModelResult);
    }

    /// <summary>
    /// Gets a specific customer by ID
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <returns>Customer details</returns>
    [HttpGet("{id}")]
    public ActionResult<CustomerViewModel> GetById(int id)
    {
        var customer = _repository.GetById(id);
        
        if (customer == null)
        {
            return NotFound($"Customer with ID {id} not found");
        }

        return Ok(ConvertToViewModel(customer));
    }

    /// <summary>
    /// Gets pagination statistics
    /// </summary>
    /// <returns>Total count and other statistics</returns>
    [HttpGet("stats")]
    public ActionResult<object> GetStats()
    {
        var totalCustomers = _repository.GetTotalCustomersCount();
        
        return Ok(new
        {
            TotalCustomers = totalCustomers,
            DefaultPageSize = DefaultPageSize,
            MaxPageSize = MaxPageSize
        });
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
