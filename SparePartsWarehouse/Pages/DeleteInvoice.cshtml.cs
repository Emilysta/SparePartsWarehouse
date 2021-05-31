using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparePartsWarehouse.DatabaseClasses;

namespace SparePartsWarehouse.Pages
{

    public class DeleteInvoiceModel : PageModel
    {
        private readonly SparePartsWarehouse.ModelContext _context;
        
        public Invoice invoice { get; set; }
        public IList<CartItem> invoiceDetails { get; set; }

        public DeleteInvoiceModel(SparePartsWarehouse.ModelContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            string s_id = HttpContext.Request.Query["id"].ToString();
            int.TryParse(s_id, out int id);
            invoice = _context.Invoices.Where(x => x.InvoiceId == id).First();
            var invoiceProducts = await _context.InvoiceItems.Where(x => x.InvoiceId == id).ToListAsync();
            invoiceDetails = new List<CartItem>();
            foreach (var prod in invoiceProducts)
            {
                invoiceDetails.Add(new CartItem
                {
                    ProductName = _context.Products.Where(x => x.ProductId == prod.ProductId).First().ProductName,
                    Count = (int)prod.ProductQuantity
                });
            }
            _context.InvoiceItems.RemoveRange(invoiceProducts);
            _context.Invoices.Remove(invoice);
            _context.SaveChanges();
            _context.Dispose();
        }
    }
}
