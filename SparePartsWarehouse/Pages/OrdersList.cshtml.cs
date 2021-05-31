using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparePartsWarehouse.DatabaseClasses;

namespace SparePartsWarehouse.Pages
{
    public class OrdersListModel : PageModel
    {
        private readonly SparePartsWarehouse.ModelContext _context;

        public OrdersListModel(SparePartsWarehouse.ModelContext context)
        {
            _context = context;
        }

        public IList<Invoice> Invoice { get; set; }

        public async Task OnGetAsync()
        {
            if (HttpContext.Session.GetString("user") == "admin")
                Invoice = await _context.Invoices.ToListAsync();
            else
                Redirect("/Index");
        }
    }
}
