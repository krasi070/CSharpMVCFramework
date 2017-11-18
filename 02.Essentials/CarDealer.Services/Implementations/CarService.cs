namespace CarDealer.Services.Implementations
{
    using Data;
    using Models;
    using Models.Cars;
    using System.Collections.Generic;
    using System.Linq;
    
    public class CarService : ICarService
    {
        private readonly CarDealerDbContext db;

        public CarService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<CarModel> All()
            => this.db.Cars
                .Select(c => new CarModel()
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .ToList();

        public IEnumerable<CarModel> ByMake(string make)
        {
            return this.db.Cars
                .Where(c => c.Make.ToLower() == make.ToLower())
                .OrderBy(c => c.Model)
                .ThenBy(c => c.TravelledDistance)
                .Select(c => new CarModel()
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .ToList();
        }

        public CarWithPartsModel WithParts(int id)
            => this.db.Cars
                .Where(c => c.Id == id)
                .Select(c => new CarWithPartsModel()
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = c.Parts.Select(p => new PartModel()
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price
                    })
                })
                .FirstOrDefault();
    }
}