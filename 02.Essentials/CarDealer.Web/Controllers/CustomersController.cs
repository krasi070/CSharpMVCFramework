﻿namespace CarDealer.Web.Controllers
{
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Models.Customers;
    using Services;
    using Services.Models;

    [Route("customers")]
    public class CustomersController : Controller
    {
        private readonly ICustomerService customers;

        public CustomersController(ICustomerService customers)
        {
            this.customers = customers;
        }

        [Route("all/{order}")]
        public IActionResult All(string order)
        {
            var orderDirection = 
                order.ToLower() == "descending" ?
                OrderDirection.Descending :
                OrderDirection.Ascending;

            var customers = this.customers.OrderedCustomers(orderDirection);

            return View(new AllCustomersModel()
            {
                Customers = customers,
                OrderDirection = orderDirection
            });
        }

        [Route("{id}")]
        public IActionResult ById(int id)
            => this.ViewOrNotFound(customers.GetCustomerById(id));

        [Route(nameof(Create))]
        public IActionResult Create() => View();

        [HttpPost]
        [Route(nameof(Create))]
        public IActionResult Create(CustomerFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.customers.Create(model.Name, model.Birthday, model.IsYoungDriver);

            return RedirectToAction(nameof(All), new { order = OrderDirection.Ascending });
        }

        [Route(nameof(Edit) + "/{id}")]
        public IActionResult Edit(int id)
        {
            var customer = this.customers.ById(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(new CustomerFormModel()
            {
                Name = customer.Name,
                Birthday = customer.Birthday,
                IsYoungDriver = customer.IsYoungDriver
            });
        }

        [HttpPost]
        [Route(nameof(Edit) + "/{id}")]
        public IActionResult Edit(int id, CustomerFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!this.customers.Exists(id))
            {
                return NotFound();
            }

            this.customers.Edit(id, model.Name, model.Birthday, model.IsYoungDriver);

            return RedirectToAction(nameof(All), new { order = OrderDirection.Ascending });
        }
    }
}