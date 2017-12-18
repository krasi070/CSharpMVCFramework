﻿namespace CarDealer.Services.Implementations
{
    using Data;
    using Models.Cars;
    using Models.Sales;
    using System.Collections.Generic;
    using System.Linq;

    public class SaleService : ISaleService
    {
        private readonly CarDealerDbContext db;

        public SaleService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<SaleListModel> All()
            => this.db.Sales
                .OrderByDescending(s => s.Id)
                .Select(s => new SaleListModel()
                {
                    Id = s.Id,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.Parts.Sum(p => p.Part.Price),
                    IsYoungDriver = s.Customer.IsYoungDriver,
                    Discount = s.Discount
                })
                .ToList();

        public IEnumerable<SaleListModel> Discounted()
            => this.db.Sales
                .Where(s => s.Discount > 0)
                .OrderByDescending(s => s.Id)
                .Select(s => new SaleListModel()
                {
                    Id = s.Id,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.Parts.Sum(p => p.Part.Price),
                    IsYoungDriver = s.Customer.IsYoungDriver,
                    Discount = s.Discount
                })
                .ToList();

        public IEnumerable<SaleListModel> DiscountedByPercent(int percent)
            => this.db.Sales
                .OrderByDescending(s => s.Id)
                .Where(s => s.Discount == (double)percent / 100)
                .Select(s => new SaleListModel()
                {
                    Id = s.Id,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.Parts.Sum(p => p.Part.Price),
                    IsYoungDriver = s.Customer.IsYoungDriver,
                    Discount = s.Discount
                })
                .ToList();

        public SaleDetailsModel Details(int id)
            => this.db.Sales
            .Where(s => s.Id == id)
            .Select(s => new SaleDetailsModel()
            {
                Id = s.Id,
                CustomerName = s.Customer.Name,
                Price = s.Car.Parts.Sum(p => p.Part.Price),
                IsYoungDriver = s.Customer.IsYoungDriver,
                Discount = s.Discount,
                Car = new CarModel()
                {
                    Make = s.Car.Make,
                    Model = s.Car.Model,
                    TravelledDistance = s.Car.TravelledDistance
                }
            })
            .FirstOrDefault();
    }
}