using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparePartsWarehouse;

namespace SparePartsWarehouse.Pages
{
    public struct InvoiceDetail
    {
        public string DetailName;
        public int Quantity;
    }
    public class InvoiceDetailsModel : PageModel
    {
        private readonly SparePartsWarehouse.ModelContext _context;
        
        public IList<InvoiceDetail> invoiceDetails { get; set; }

        public InvoiceDetailsModel(SparePartsWarehouse.ModelContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            string s_id = HttpContext.Request.Query["id"].ToString();
            int.TryParse(s_id, out int id);
            var invoiceProducts = await _context.InvoiceItems.Where(x => x.InvoiceId == id).ToListAsync();
            invoiceDetails = new List<InvoiceDetail>();
            foreach(var prod in invoiceProducts)
            {
                invoiceDetails.Add(new InvoiceDetail
                { 
                    DetailName = _context.Products.Where(x=> x.ProductId == prod.ProductId).First().ProductName,
                    Quantity = (int)prod.ProductQuantity
                });
            }
            _context.Dispose();
        }
    }
}
