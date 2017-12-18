namespace CarDealer.Services.Implementations
{
    using Data;
    using Models;
    using Models.Sales;
    using Models.Customers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Data.Models;

    public class CustomerService : ICustomerService
    {
        private readonly CarDealerDbContext db;

        public CustomerService(CarDealerDbContext db)
        {
            this.db = db;
        }

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
                    Id = c.Id,
                    Name = c.Name,
                    Birthday = c.Birthday,
                    IsYoungDriver = c.IsYoungDriver
                })
                .ToList();
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

        public CustomerModel ById(int id)
            => this.db.Customers
                .Where(c => c.Id == id)
                .Select(c => new CustomerModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Birthday = c.Birthday,
                    IsYoungDriver = c.IsYoungDriver
                })
                .FirstOrDefault();

        public bool Exists(int id)
            => this.db.Customers.Any(c => c.Id == id);

        public void Create(string name, DateTime birthday, bool isYoungDriver)
        {
            var customer = new Customer()
            {
                Name = name,
                Birthday = birthday,
                IsYoungDriver = isYoungDriver
            };

            this.db.Add(customer);
            this.db.SaveChanges();
        }

        public void Edit(int id, string name, DateTime birthday, bool isYoungDriver)
        {
            var customer = this.db.Customers.Find(id);
            if (customer == null)
            {
                return;
            }

            customer.Name = name;
            customer.Birthday = birthday;
            customer.IsYoungDriver = isYoungDriver;

            this.db.SaveChanges();
        }
    }
}