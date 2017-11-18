namespace CarDealer.Web.Controllers
{
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Services;

    [Route("sales")]
    public class SalesController : Controller
    {
        private const string SalesView = "Sales";

        private readonly ISaleService sales;

        public SalesController(ISaleService sales)
        {
            this.sales = sales;
        }

        [Route("")]
        public IActionResult All()
        {
            this.ViewData["Title"] = "All Sales";
            return View(SalesView, this.sales.All());
        }

        [Route("discounted")]
        public IActionResult Discounted()
        {
            this.ViewData["Title"] = "Discounted Sales";
            return View(SalesView, this.sales.Discounted());
        }

        [Route("discounted/{percent}")]
        public IActionResult Discounted(int percent)
        {
            this.ViewData["Title"] = $"Sales Discounted by {percent}%";
            return View(SalesView, this.sales.DiscountedByPercent(percent));
        }

        [Route("{id}")]
        public IActionResult Details(int id)
            => this.ViewOrNotFound(this.sales.Details(id));
    }
}