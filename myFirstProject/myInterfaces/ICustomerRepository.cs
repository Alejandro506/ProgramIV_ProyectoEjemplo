using myFirstProject.Models;
using System.Collections.Generic;

namespace myFirstProject.Repository;

public interface ICustomerRepository
{
    public IEnumerable<Customer> GetCustomers(int maxNumberOfRecords);
    public IEnumerable<Customer> QueryByName(string name, int maxNumberOfRecords);
    public Customer? GetById(int customerID);
}

