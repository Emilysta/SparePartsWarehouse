using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace SparePartsWarehouse
{
    public struct SparePart
    {
        public string DetailName;
        public int RequiredQuantity;
    }
    public class SparePartsModel : PageModel
    {
        private readonly ModelContext _context;

        public IList<SparePart> spareParts { get; set; }

        public SparePartsModel(ModelContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get; set; }

        public async Task OnGetAsync(int id)
        {
            var prodSpecifications = await _context.ProdSpecifications.Where(x => x.ProductId == id).ToListAsync();
            spareParts = new List<SparePart>();
            foreach (var prod in prodSpecifications)
            {
                spareParts.Add(new SparePart
                {
                    DetailName = _context.Details.Where(x => x.DetailId == prod.DetailId).FirstOrDefault().DetailName,
                    RequiredQuantity = (int)prod.DetailQuantity
                });
            }
            _context.Dispose();
        }
    }
}
