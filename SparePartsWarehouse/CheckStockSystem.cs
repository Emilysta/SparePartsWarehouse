using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SparePartsWarehouse.DatabaseClasses;

namespace SparePartsWarehouse
{
    public class CheckStockSystem
    {
        public static int howManyToBe = 50;
        public static void CheckWeeklyStock()
        {
            ModelContext m_context = new ModelContext();
            List<Detail> allDetails = m_context.Details.ToList();
            foreach(Detail detail in allDetails)
            {
                Stock stock = m_context.Stocks.Where(x => x.DetailId == detail.DetailId).First();
                if (stock!=null)
                {
                    if(stock.Quantity<howManyToBe)
                    {
                        stock.Quantity = howManyToBe;
                    }
                }
                else
                {
                    m_context.Stocks.Add(new Stock(){ 
                        DetailId = detail.DetailId, 
                        DetailName = detail.DetailName, 
                        Quantity = howManyToBe 
                    });
                }
            }
            m_context.SaveChanges();
            m_context.Dispose();
        }

        public static void CheckTwiceADay()
        {
            ModelContext m_context = new ModelContext();
            List<Stock> neededDetails = GetAllDetailsFromInvoices();
            foreach (Stock stockDetail in neededDetails)
            {
                Stock stock = m_context.Stocks.Where(x => x.DetailId == stockDetail.DetailId).First();
                if (stock != null)
                {
                    if (stock.Quantity < stockDetail.Quantity)
                    {
                        stock.Quantity = stockDetail.Quantity;
                    }
                }
                else
                {
                    m_context.Stocks.Add(new Stock()
                    {
                        DetailId = stockDetail.DetailId,
                        DetailName = stockDetail.DetailName,
                        Quantity = stockDetail.Quantity
                    });
                }
            }
            m_context.SaveChanges();
            m_context.Dispose();
        }

        private static List<Stock> GetAllDetailsFromInvoices()
        {
            ModelContext m_context = new ModelContext();
            List<InvoiceItem> allInvoiceItems = new List<InvoiceItem>();
            List<Invoice> invoiceList = m_context.Invoices.ToList();
            foreach (Invoice item in invoiceList)
            {
                List<InvoiceItem> tempList = m_context.InvoiceItems.Where(x => x.InvoiceId == item.InvoiceId).ToList();
                foreach (InvoiceItem invoiceItem in tempList)
                {
                    if (allInvoiceItems.Any(x => x.ProductId == invoiceItem.ProductId))
                    {
                        var itemToChange = allInvoiceItems.Where(x => x.ProductId == invoiceItem.ProductId).FirstOrDefault();
                        itemToChange.ProductQuantity += invoiceItem.ProductQuantity;
                    }
                    else
                    {
                        allInvoiceItems.Add(invoiceItem);
                    }
                }
            }

            List<Stock> details = new List<Stock>();

            foreach (InvoiceItem item in allInvoiceItems)
            {
                List<ProdSpecification> prodSpecList = m_context.ProdSpecifications.Where(x => x.ProductId == item.ProductId).ToList();
                foreach (ProdSpecification prodDetail in prodSpecList)
                {
                    if (details.Any(x => x.DetailId == prodDetail.DetailId))
                    {
                        var detailItem = details.Find(x => x.DetailId == prodDetail.DetailId);
                        detailItem.Quantity += (int)(prodDetail.DetailQuantity * item.ProductQuantity);
                    }
                    else
                    {
                        details.Add(new Stock()
                        {
                            DetailName = m_context.Details.Where(x => x.DetailId == prodDetail.DetailId).FirstOrDefault().DetailName,
                            DetailId = (int)prodDetail.DetailId,
                            Quantity = (int)(prodDetail.DetailQuantity * item.ProductQuantity)
                        });
                    }
                }
            }

            return details;
        }
    }
}
