using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SparePartsWarehouse.Pages;

namespace SparePartsWarehouse
{
    public static class OrderSystem
    {
        /// <summary>
        /// Event informs about a placed order. When it is raised, the databse
        /// has been updated with a new invoice.
        /// </summary>
        public static event Action OrderMadeEvent;

        /// <summary>
        /// Method allows to make an order. It runs on a separate thread and saves
        /// the needed data to the database.
        /// </summary>
        /// <param name="purchaser">Purchaser to be put on invoice</param>
        /// <param name="orderItems">List of items to be ordered</param>
        public static void MakeOrder(string purchaser, List<CartItem> orderItems)
        {
            new Thread(() =>
            {
                ModelContext _context = new ModelContext();
                var invoice = _context.Invoices.Add(new Invoice
                {
                    Purchaser = purchaser,
                    InvoiceDate = DateTime.Now
                });
                _context.SaveChanges();

                decimal invoiceId = _context.Invoices.OrderByDescending(x => x.InvoiceId).Select(x => x.InvoiceId).FirstOrDefault();

                foreach (CartItem item in orderItems)
                {
                    _context.InvoiceItems.Add(new InvoiceItem
                    {
                        ProductId = item.ProductId,
                        ProductQuantity = item.Count,
                        InvoiceId = invoiceId,
                        Invoice = invoice.Entity,
                        Product = _context.Products.Where(x => x.ProductId == item.ProductId).First()
                    });
                }

                _context.SaveChanges();
                _context.Dispose();
                OrderMadeEvent?.Invoke();
            }).Start();

        }
    }
}
