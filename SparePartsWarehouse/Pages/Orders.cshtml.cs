using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SparePartsWarehouse
{
    public class OrdersModel : PageModel
    {
        private readonly ModelContext _context;

        [Required]
        [StringLength(80, MinimumLength = 3)]
        //[RegularExpression("^[a - zA - Z] + (([',. -][a-zA-Z ])?[a-zA-Z]*)*$",ErrorMessage ="Dozwolone znaki to a-z, A-Z")]
        public string Purchaser { get; set; }

        [Required]
        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Ilość może zawierać tylko liczby większe od 0")]
        public int Quantity { get; set; }

        public SelectList Products { get; set; }

        public OrdersModel(ModelContext context) => _context = context;
        public void OnGet()
        {
            Products = new SelectList(_context.Products.Select(x => x.ProductName).ToList());
            _context.Dispose();
        }
    }
}
