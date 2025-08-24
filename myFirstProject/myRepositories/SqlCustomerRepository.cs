using Microsoft.EntityFrameworkCore;
using myFirstProject.Models;
using myFirstProject.MyModels;
using myFirstProject.Data;

namespace myFirstProject.Repository;

public class SqlCustomerRepository : ICustomerRepository
{
    private readonly Data.AdventureWorksContext _context;

    public SqlCustomerRepository(Data.AdventureWorksContext context)
    {
        _context = context;
    }

    public IEnumerable<Customer> GetCustomers(int maxNumberOfRecords)
    {
        return _context.Customers.AsNoTracking()
            .Take(maxNumberOfRecords)
            .ToList();
    }

    public IEnumerable<Customer> QueryByName(string name, int maxNumberOfRecords)
    {
        return _context.Customers.AsNoTracking()
            .Where(c => (!string.IsNullOrEmpty(c.FirstName) && c.FirstName.Contains(name)) ||
                        (!string.IsNullOrEmpty(c.MiddleName) && c.MiddleName.Contains(name)) ||
                        (!string.IsNullOrEmpty(c.LastName) && c.LastName.Contains(name)))
            .Take(maxNumberOfRecords)
            .ToList();
    }

    public Customer? GetById(int customerID)
    {
        return _context.Customers.AsNoTracking()
            .FirstOrDefault(c => c.CustomerID == customerID);
    }

    // New pagination methods
    public PaginatedResult<Customer> GetCustomersPaginated(int pageNumber, int pageSize)
    {
        var totalItems = GetTotalCustomersCount();
        var customers = _context.Customers.AsNoTracking()
            .OrderBy(c => c.CustomerID) // Consistent ordering for pagination
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return PaginatedResult<Customer>.Create(customers, totalItems, pageNumber, pageSize);
    }

    public PaginatedResult<Customer> QueryByNamePaginated(string name, int pageNumber, int pageSize)
    {
        var totalItems = GetTotalCustomersCountByName(name);
        var customers = _context.Customers.AsNoTracking()
            .Where(c => (!string.IsNullOrEmpty(c.FirstName) && c.FirstName.Contains(name)) ||
                        (!string.IsNullOrEmpty(c.MiddleName) && c.MiddleName.Contains(name)) ||
                        (!string.IsNullOrEmpty(c.LastName) && c.LastName.Contains(name)))
            .OrderBy(c => c.CustomerID) // Consistent ordering for pagination
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return PaginatedResult<Customer>.Create(customers, totalItems, pageNumber, pageSize);
    }

    public int GetTotalCustomersCount()
    {
        return _context.Customers.Count();
    }

    public int GetTotalCustomersCountByName(string name)
    {
        return _context.Customers
            .Where(c => (!string.IsNullOrEmpty(c.FirstName) && c.FirstName.Contains(name)) ||
                        (!string.IsNullOrEmpty(c.MiddleName) && c.MiddleName.Contains(name)) ||
                        (!string.IsNullOrEmpty(c.LastName) && c.LastName.Contains(name)))
            .Count();
    }
}

