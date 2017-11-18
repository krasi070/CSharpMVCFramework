namespace CarDealer.Services
{
    using Models.Sales;
    using System.Collections.Generic;

    public interface ISaleService
    {
        IEnumerable<SaleListModel> All();

        IEnumerable<SaleListModel> Discounted();

        IEnumerable<SaleListModel> DiscountedByPercent(int percent);

        SaleDetailsModel Details(int id);
    }
}