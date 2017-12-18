﻿namespace CarDealer.Web.Controllers
{
    using Models.Cars;
    using Microsoft.AspNetCore.Mvc;
    using Services;

    [Route("cars")]
    public class CarsController : Controller
    {
        private const string CarsView = "Cars";

        private readonly ICarService cars;

        public CarsController(ICarService cars)
        {
            this.cars = cars;
        }

        [Route("all")]
        public IActionResult All()
        {
            ViewData["Title"] = $"All Cars";

            return View(CarsView, this.cars.All());
        }

        [Route(nameof(Create))]
        public IActionResult Create() => View();

        [HttpPost]
        [Route(nameof(Create))]
        public IActionResult Create(CarFormModel carModel)
        {
            if (!ModelState.IsValid)
            {
                return View(carModel);
            }

            this.cars.Create(carModel.Make, carModel.Model, carModel.TravelledDistance);

            return RedirectToAction(nameof(All));
        }

        [Route("{make}")]
        public IActionResult ByMake(string make)
        {
            ViewData["Title"] = $"{make} Cars";

            return View(CarsView, this.cars.ByMake(make));
        }

        [Route("{id}/parts")]
        public IActionResult Parts(int id)
        {
            var car = this.cars.WithParts(id);
            if (car == null)
            {
                return NotFound();
            }

            ViewData["Title"] = $"{car.Model} {car.Make} Details";

            return View("Parts", car);
        }
    }
}