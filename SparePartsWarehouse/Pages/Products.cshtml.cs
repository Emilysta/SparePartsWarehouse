using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SparePartsWarehouse
{
    public class ProductsModel : PageModel
    {
        private readonly ModelContext _context;

        [BindProperty(SupportsGet = true)]

        public string SearchString { get; set; }

        public ProductsModel(ModelContext context) => _context = context;

        public IList<Product> ProductsList { get; set; }

        public int PageNumber { get; set; }

        public void OnGet(int page)
        {
            if (string.IsNullOrEmpty(SearchString))
            {
                ProductsList = _context.Products.OrderBy(x => x.ProductId).ToList();
            }
            else
            {
                ProductsList = _context.Products.Where(x => x.ProductName.Contains(SearchString)).OrderBy(x => x.ProductId).ToList();
            }
            _context.Dispose();
            PageNumber = page;
        }
    }
}
