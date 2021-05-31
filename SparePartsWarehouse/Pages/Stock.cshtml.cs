using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using SparePartsWarehouse.DatabaseClasses;

namespace SparePartsWarehouse.Pages
{
    public class StockModel : PageModel
    {
        private readonly ModelContext _context;

        public StockModel(ModelContext context) => _context = context;

        public IList<Stock> Stock { get; set; }

        public async Task OnGetAsync()
        {
            Stock = await _context.Stocks.ToListAsync();
        }

        public async Task OnPostAsync()
        {
            Request.Form.TryGetValue("Detail", out StringValues detailValues);
            Request.Form.TryGetValue("Quantity", out StringValues QuantityValues);

            List<DetailOrderItem> orderItems = new List<DetailOrderItem>();
            int i = 0;
            foreach (string s in detailValues)
            {
                orderItems.Add(new DetailOrderItem
                {
                    ItemId = int.Parse(s),
                    ItemName = _context.Details.Where(x => x.DetailId == int.Parse(s)).Select(x => x.DetailName).Single(),
                    Quantity = int.Parse(QuantityValues[i])
                });
                i++;
            }

            DetailOrderSystem.MakeDetailOrder(orderItems);
            var stockTask = _context.Stocks.ToListAsync();
            Stock = await stockTask;
        }
    }
}
