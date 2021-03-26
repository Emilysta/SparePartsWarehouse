using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace SparePartsWarehouse
{
    public class ProductsModel : PageModel
    {
        private readonly ModelContext _context;

        [BindProperty(SupportsGet = true)]

        public string SearchString { get; set; }

        public ProductsModel(ModelContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; }

        public async Task OnGetAsync()
        {
            if (string.IsNullOrEmpty(SearchString))
            {
                Product = await _context.Products.OrderBy(x => x.ProductId).ToListAsync();
            }
            else
            {
                Product = await _context.Products.Where(x=>x.ProductName.Contains(SearchString)).OrderBy(x => x.ProductId).ToListAsync();
            }
            _context.Dispose();
        }
    }
}
