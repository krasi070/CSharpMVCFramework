namespace CarDealer.Web.Controllers
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
    }
}