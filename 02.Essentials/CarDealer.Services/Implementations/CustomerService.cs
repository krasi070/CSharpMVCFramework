namespace CarDealer.Services.Implementations
{
    using Data;
    using Models;
    using Models.Sales;
    using Models.Customers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CustomerService : ICustomerService
    {
        private readonly CarDealerDbContext db;

        public CustomerService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public CustomerTotalSalesModel GetCustomerById(int id)
            => this.db.Customers
                    .Where(c => c.Id == id)
                    .Select(c => new CustomerTotalSalesModel()
                    {
                        Name = c.Name,
                        IsYoungDriver = c.IsYoungDriver,
                        BoughtCars = c.Sales.Select(s => new SaleModel()
                        {
                            Price = s.Car.Parts.Sum(p => p.Part.Price),
                            Discount = s.Discount
                        })
                    })
                    .FirstOrDefault();

        public IEnumerable<CustomerModel> OrderedCustomers(OrderDirection order)
        {
            var customersQuery = this.db.Customers.AsQueryable();

            switch (order)
            {
                case OrderDirection.Ascending:
                    customersQuery = customersQuery
                        .OrderBy(c => c.Birthday)
                        .ThenBy(c => c.Name);
                    break;
                case OrderDirection.Descending:
                    customersQuery = customersQuery
                        .OrderByDescending(c => c.Birthday)
                        .ThenBy(c => c.Name); ;
                    break;
                default:
                    throw new InvalidOperationException($"Invalid order direction: {order}!");
            }

            return customersQuery
                .Select(c => new CustomerModel()
                {
                    Name = c.Name,
                    Birthday = c.Birthday,
                    IsYoungDriver = c.IsYoungDriver
                })
                .ToList();
        }
    }
}