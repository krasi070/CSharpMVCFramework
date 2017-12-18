namespace CarDealer.Services
{
    using Models.Cars;
    using System.Collections.Generic;

    public interface ICarService
    {
        IEnumerable<CarModel> All();

        IEnumerable<CarModel> ByMake(string make);

        CarWithPartsModel WithParts(int id);

        void Create(string make, string model, long travelledDistance);
    }
}