using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using SparePartsWarehouse;

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
