using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SparePartsWarehouse.DatabaseClasses;

namespace SparePartsWarehouse
{
    public struct DetailOrderItem
    {
        public string ItemName;
        public int ItemId;
        public int Quantity;
    }
    public class DetailOrderSystem
    {
        /// <summary>
        /// Method allows to make an order of detail when user is admin.
        /// Method adds the requested details to the current stock.
        /// </summary>
        /// <param name="orderItems">List of items to be ordered</param>
        public static void MakeDetailOrder(List<DetailOrderItem> orderItems)
        {
            ModelContext _context = new ModelContext();

            foreach (DetailOrderItem item in orderItems)
            {
                if (_context.Stocks.Any(x => x.DetailId == item.ItemId))
                {
                    var stockDetail = _context.Stocks.Where(x => x.DetailId == item.ItemId).FirstOrDefault();
                    stockDetail.Quantity += item.Quantity;
                }
                else
                {
                    _context.Stocks.Add(new Stock
                    {
                        DetailId = item.ItemId,
                        DetailName = item.ItemName,
                        Quantity = item.Quantity
                    });
                }
            }
            _context.SaveChanges();
            _context.Dispose();
        }
    }
}
