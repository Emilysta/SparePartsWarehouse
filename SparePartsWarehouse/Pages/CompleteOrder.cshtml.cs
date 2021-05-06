using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using SparePartsWarehouse.DatabaseClasses;

namespace SparePartsWarehouse.Pages
{
    public class CompleteOrderModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int InvoiceID { get; set; }

        public string Purchaser { get; set; }

        public List<CartItem> OrderItemsList { get; set; }

        public List<Stock> OrderDetailList { get; set; }

        private ModelContext m_context;
        public CompleteOrderModel(ModelContext context) => m_context = context;
        public void OnGet()
        {
            Purchaser = m_context.Invoices.Where(x => x.InvoiceId == InvoiceID).Select(x => x.Purchaser).FirstOrDefault();
            OrderItemsList = new List<CartItem>();
            OrderDetailList = new List<Stock>();
            foreach (var product in m_context.InvoiceItems.Where(x => x.InvoiceId == InvoiceID))
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
            using (var connection = m_context.Database.GetDbConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand() as OracleCommand;
                cmd.CommandText = "COMPLETE_ORDER";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                OracleParameter param1 = new OracleParameter("ORDER_INVOICE_ID", InvoiceID);
                cmd.Parameters.Add(param1);

                var i = cmd.ExecuteNonQuery();
                Debug.WriteLine("Rows affected: " + i);
                connection.Close();
            }
            m_context.SaveChanges();
        }
    }
}
