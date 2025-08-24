using Microsoft.AspNetCore.Mvc;
using myFirstProject.Models;
using myFirstProject.MyModels;
using myFirstProject.Repository;
using myFirstProject.ViewModels;

namespace myFirstProject.Controllers;

public class PagedCustomersMvcController : Controller
{
    private readonly ICustomerRepository _repository;

    public PagedCustomersMvcController(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        return View(new PagedCustomersViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Search(PagedCustomersViewModel viewModel)
    {
        return ExecuteSearch(viewModel);
    }

    [HttpGet]
    public IActionResult Search(string name, int pageNumber = 1, int pageSize = 10)
    {
        var viewModel = new PagedCustomersViewModel
        {
            Name = name,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        
        return ExecuteSearch(viewModel);
    }

    private IActionResult ExecuteSearch(PagedCustomersViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var result = string.IsNullOrWhiteSpace(viewModel.Name)
                    ? _repository.GetCustomersPaginated(viewModel.PageNumber, viewModel.PageSize)
                    : _repository.QueryByNamePaginated(viewModel.Name, viewModel.PageNumber, viewModel.PageSize);

                viewModel.Results = new PaginatedResult<CustomerViewModel>
                {
                    Items = result.Items.Select(ConvertToViewModel),
                    Pagination = result.Pagination
                };
            }
            catch (Exception ex)
            {
                viewModel.ErrorMessage = ex.Message;
            }
        }
        else
        {
            viewModel.ErrorMessage = "Invalid search criteria.";
        }

        return View("Index", viewModel);
    }

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
        };
    }
}
