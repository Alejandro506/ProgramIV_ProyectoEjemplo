using System.Text.Json;
using myFirstProject.MyModels;
using myFirstProject.Models;

namespace myFirstProject.Repository;

public class JsonCustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers;

    public JsonCustomerRepository(string jsonFilePath)
    {
        if (File.Exists(jsonFilePath))
        {
            var jsonData = File.ReadAllText(jsonFilePath);
            _customers = JsonSerializer.Deserialize<List<Customer>>(jsonData) ?? new List<Customer>();
        }
        else
        {
            _customers = new List<Customer>();
        }
    }

    public IEnumerable<Customer> GetCustomers(int maxNumberOfRecords)
    {
        return _customers.Take(maxNumberOfRecords);
    }

    public IEnumerable<Customer> QueryByName(string name, int maxNumberOfRecords)
    {
        return _customers
            .Where(c => (!string.IsNullOrEmpty(c.FirstName) && c.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(c.MiddleName) && c.MiddleName.Contains(name, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(c.LastName) && c.LastName.Contains(name, StringComparison.OrdinalIgnoreCase)))
            .Take(maxNumberOfRecords);
    }

    public Customer? GetById(int customerId)
    {
        return _customers.FirstOrDefault(c => c.CustomerID == customerId);
    }

    // New pagination methods
    public PaginatedResult<Customer> GetCustomersPaginated(int pageNumber, int pageSize)
    {
        var totalItems = GetTotalCustomersCount();
        var customers = _customers
            .OrderBy(c => c.CustomerID) // Consistent ordering for pagination
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return PaginatedResult<Customer>.Create(customers, totalItems, pageNumber, pageSize);
    }

    public PaginatedResult<Customer> QueryByNamePaginated(string name, int pageNumber, int pageSize)
    {
        var totalItems = GetTotalCustomersCountByName(name);
        var customers = _customers
            .Where(c => (!string.IsNullOrEmpty(c.FirstName) && c.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(c.MiddleName) && c.MiddleName.Contains(name, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(c.LastName) && c.LastName.Contains(name, StringComparison.OrdinalIgnoreCase)))
            .OrderBy(c => c.CustomerID) // Consistent ordering for pagination
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return PaginatedResult<Customer>.Create(customers, totalItems, pageNumber, pageSize);
    }

    public int GetTotalCustomersCount()
    {
        return _customers.Count;
    }

    public int GetTotalCustomersCountByName(string name)
    {
        return _customers
            .Where(c => (!string.IsNullOrEmpty(c.FirstName) && c.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(c.MiddleName) && c.MiddleName.Contains(name, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(c.LastName) && c.LastName.Contains(name, StringComparison.OrdinalIgnoreCase)))
            .Count();
    }
}
