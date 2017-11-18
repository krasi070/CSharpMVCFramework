namespace CarDealer.Services
{
    using Models;
    using Models.Customers;
    using System.Collections.Generic;

    public interface ICustomerService
    {
        IEnumerable<CustomerModel> OrderedCustomers(OrderDirection order);

        CustomerTotalSalesModel GetCustomerById(int id);
    }
}