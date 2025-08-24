using myFirstProject.Models;
using myFirstProject.MyModels;
using System.Collections.Generic;

namespace myFirstProject.Repository;

public interface ICustomerRepository
{
    // Original methods (preserved for backward compatibility)
    public IEnumerable<Customer> GetCustomers(int maxNumberOfRecords);
    public IEnumerable<Customer> QueryByName(string name, int maxNumberOfRecords);
    public Customer? GetById(int customerID);
    
    // New pagination methods
    public PaginatedResult<Customer> GetCustomersPaginated(int pageNumber, int pageSize);
    public PaginatedResult<Customer> QueryByNamePaginated(string name, int pageNumber, int pageSize);
    public int GetTotalCustomersCount();
    public int GetTotalCustomersCountByName(string name);
}

