namespace CarDealer.Services
{
    using Models;
    using Models.Customers;
    using System;
    using System.Collections.Generic;

    public interface ICustomerService
    {
        IEnumerable<CustomerModel> OrderedCustomers(OrderDirection order);

        CustomerTotalSalesModel GetCustomerById(int id);

        void Create(string name, DateTime birthday, bool isYoungDriver);

        CustomerModel ById(int id);

        void Edit(int id, string name, DateTime birthday, bool isYoungDriver);

        bool Exists(int id);
    }
}