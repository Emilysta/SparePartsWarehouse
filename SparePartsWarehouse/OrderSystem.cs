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
        public static event EventHandler OrderMadeEvent = delegate { };

        /// <summary>
        /// Method allows to make an order. It runs on a separate thread and saves
        /// the needed data to the database.
        /// </summary>
        /// <param name="purchaser">Purchaser to be put on invoice</param>
        /// <param name="orderItems">List of items to be ordered</param>
        public static void MakeOrder(string purchaser, List<OrderItem> orderItems)
        {
            Thread thread = new Thread(() =>
            {
                ModelContext _context = new ModelContext();
                _context.Invoices.Add(new Invoice
                {
                    Purchaser = purchaser,
                    InvoiceDate = DateTime.Now
                });
                _context.SaveChanges();
                decimal invoiceId = _context.Invoices.OrderByDescending(x => x.InvoiceId).Select(x => x.InvoiceId).FirstOrDefault();
                foreach (OrderItem item in orderItems)
                {
                    decimal productId = _context.Products.Where(x => x.ProductName == item.ItemName).Select(x => x.ProductId).FirstOrDefault();
                    _context.InvoiceItems.Add(new InvoiceItem
                    {
                        ProductId = productId,
                        ProductQuantity = item.Quantity,
                        InvoiceId = invoiceId,
                        Invoice = _context.Invoices.OrderByDescending(x => x.InvoiceId).First(),
                        Product = _context.Products.Where(x => x.ProductId == productId).First()
                    });
                }
                _context.SaveChanges();
                _context.Dispose();
                OrderMadeEvent(null, EventArgs.Empty);
            });

        }
    }
}
