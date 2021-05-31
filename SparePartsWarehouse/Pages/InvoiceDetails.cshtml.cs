using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SparePartsWarehouse.DatabaseClasses;

namespace SparePartsWarehouse.Pages
{
    public class InvoiceDetailsModel : PageModel
    {
        public List<CartItem> OrderItemsList { get; set; }
        public List<Stock> OrderDetailList { get; set; }
        public List<Stock> OrderStockList { get; set; }

        private readonly SparePartsWarehouse.ModelContext m_context;

        public InvoiceDetailsModel(SparePartsWarehouse.ModelContext context) => m_context = context;

        public async Task OnGetAsync()
        {
            string s_id = HttpContext.Request.Query["id"].ToString();
            int.TryParse(s_id, out int id);
            OrderItemsList = new List<CartItem>();
            OrderDetailList = new List<Stock>();
            OrderStockList = new List<Stock>();
            foreach (var product in m_context.InvoiceItems.Where(x => x.InvoiceId == id))
            {
                OrderItemsList.Add(new CartItem
                {
                    ProductId = (int)product.ProductId,
                    ProductName = m_context.Products.Where(x => x.ProductId == product.ProductId).Select(x => x.ProductName).FirstOrDefault(),
                    Count = (int)product.ProductQuantity
                });

                foreach (var detail in m_context.ProdSpecifications.Where(x => x.ProductId == product.ProductId))
                {
                    if (OrderDetailList.Any(x => x.DetailId == detail.DetailId))
                    {
                        OrderDetailList.Find(x => x.DetailId == detail.DetailId).Quantity += (int)product.ProductQuantity * detail.DetailQuantity;
                    }
                    else
                    {
                        OrderDetailList.Add(new Stock
                        {
                            DetailId = detail.DetailId,
                            DetailName = m_context.Details.Where(x => x.DetailId == detail.DetailId).Select(x => x.DetailName).FirstOrDefault(),
                            Quantity = (int)product.ProductQuantity * detail.DetailQuantity
                        });
                    }
                }
            }
            foreach (var detail in OrderDetailList)
            {
                if (m_context.Stocks.Any(x => x.DetailId == detail.DetailId))
                    OrderStockList.Add(m_context.Stocks.Where(x => x.DetailId == detail.DetailId).FirstOrDefault());
                else
                    OrderStockList.Add(new Stock
                    {
                        DetailName = detail.DetailName,
                        Quantity = 0
                    });
            }
            m_context.Dispose();
        }
    }
}
