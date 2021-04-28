using SparePartsWarehouse.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SparePartsWarehouse
{
    public struct DetailOrderItem
    {
        public string ItemName;
        public int Quantity;
        public int ItemId;
    }
    public class DetailOrderSystem
    {
        /// <summary>
        /// Method allows to make an order of detail when user is admin. It runs on a separate thread and saves
        /// the needed data to the database.
        /// </summary>
        /// <param name="orderItems">List of items to be ordered</param>
        public static void MakeDetailOrder( List<DetailOrderItem> orderItems)
        {
            new Thread(() =>
            {
                ModelContext _context = new ModelContext();
                var invoice = _context.Stocks;
                _context.SaveChanges();
                _context.Dispose();


            }).Start();

        }
    }
}
