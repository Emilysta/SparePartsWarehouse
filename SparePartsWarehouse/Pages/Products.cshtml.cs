using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SparePartsWarehouse.DatabaseClasses;
using SparePartsWarehouse.Pages;

namespace SparePartsWarehouse
{
    public class ProductsModel : PageModel
    {
        private readonly ModelContext _context;

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; }

        public ProductsModel(ModelContext context) => _context = context;

        public IList<Product> ProductsList { get; set; }

        [BindProperty]
        [Required]
        public string ProductName { get; set; }
        [BindProperty]
        [Required]
        public int ProductID { get; set; }
        [BindProperty]
        [Required]
        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Ilość może zawierać tylko liczby większe od 0")]
        public int ProductQuantity { get; set; }

        public async void OnGet()
        {
            if (string.IsNullOrEmpty(SearchString))
            {
                ProductsList = await _context.Products.OrderBy(x => x.ProductName).ToListAsync();
            }
            else
            {
                ProductsList = await _context.Products.Where(x => x.ProductName.Contains(SearchString)).OrderBy(x => x.ProductName).ToListAsync();
                PageNumber = 1;
            }
            if (PageNumber == 0)
                PageNumber = 1;
            _context.Dispose();
        }

        public async void OnPostAsync()
        {
            if (string.IsNullOrEmpty(SearchString))
            {
                ProductsList = await _context.Products.OrderBy(x => x.ProductName).ToListAsync();
            }
            else
            {
                ProductsList = await _context.Products.Where(x => x.ProductName.Contains(SearchString)).OrderBy(x => x.ProductName).ToListAsync();
                PageNumber = 1;
            }
            if (PageNumber == 0)
                PageNumber = 1;
            _context.Dispose();

            if (ProductQuantity > 0)
            {
                List<CartItem> items;
                if (Request.Cookies.TryGetValue("CartList", out string cartListString))
                {
                    items = JsonConvert.DeserializeObject<List<CartItem>>(cartListString);
                    Response.Cookies.Delete("CartList");
                }
                else
                    items = new List<CartItem>();

                items.Add(new CartItem
                {
                    ProductName = this.ProductName,
                    ProductId = this.ProductID,
                    Count = this.ProductQuantity
                });

                int itemsCount = 0;
                foreach (CartItem x in items)
                    itemsCount += x.Count;

                Response.Cookies.Append("CartList", JsonConvert.SerializeObject(items));
                Response.Cookies.Append("CartItemsCount", itemsCount.ToString());
            }
        }
    }
}
