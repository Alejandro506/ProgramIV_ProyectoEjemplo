using Microsoft.EntityFrameworkCore;
using myFirstProject.Models;
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
}

