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
    public class StockModel : PageModel
    {
        private readonly SparePartsWarehouse.ModelContext _context;

        public StockModel(SparePartsWarehouse.ModelContext context)
        {
            _context = context;
        }

        public IList<Stock> Stock { get;set; }

        public async Task OnGetAsync()
        {
            Stock = await _context.Stocks.ToListAsync();
        }
    }
}
