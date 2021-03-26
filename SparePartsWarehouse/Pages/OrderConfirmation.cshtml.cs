using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SparePartsWarehouse.Pages
{
    public struct OrderItem
    {
        public string ItemName;
        public int Quantity;
    }
    public class OrderConfirmationModel : PageModel
    {
        private readonly ModelContext _context;
        public List<OrderItem> ProductsList { get; set; }
        public string Purchaser { get; set; }

        public OrderConfirmationModel(ModelContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
        }
        public async Task OnPostAsync()
        {
            Request.Form.TryGetValue("Purchaser", out Microsoft.Extensions.Primitives.StringValues values);
            Purchaser = values[0];
            _context.Invoices.Add(new Invoice
            {
                Purchaser = this.Purchaser,
                InvoiceDate = DateTime.Now
            });
            await _context.SaveChangesAsync();
            decimal invoiceId = _context.Invoices.OrderByDescending(x => x.InvoiceId).Select(x => x.InvoiceId).FirstOrDefault();
            Request.Form.TryGetValue("Product", out Microsoft.Extensions.Primitives.StringValues Products);
            Request.Form.TryGetValue("Quantity", out Microsoft.Extensions.Primitives.StringValues Quantities);
            ProductsList = new List<OrderItem>();
            int i = 0;
            foreach (string s in Products)
            {
                ProductsList.Add(new OrderItem
                {
                    ItemName = Products[i],
                    Quantity = int.Parse(Quantities[i])
                });
                i++;
            }
            foreach (string s in Products)
            {
                decimal productId = _context.Products.Where(x => x.ProductName == s).Select(x => x.ProductId).FirstOrDefault();
                _context.InvoiceItems.Add(new InvoiceItem
                {
                    ProductId = productId,
                    ProductQuantity = int.Parse(Quantities[i]),
                    InvoiceId = invoiceId,
                    Invoice = _context.Invoices.OrderByDescending(x => x.InvoiceId).First(),
                    Product = _context.Products.Where(x => x.ProductId == productId).First()
                });
                i++;
            }
            await _context.SaveChangesAsync();
        }
    }
}
