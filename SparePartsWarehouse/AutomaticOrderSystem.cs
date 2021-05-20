using SparePartsWarehouse.DatabaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SparePartsWarehouse
{

    public class AutomaticOrderSystem
    {

        public static void OrderWeeklyAverageConsumption()
        {
            DetailOrderSystem.MakeDetailOrder(CheckAverageConsumption());
        }

        private static List<DetailOrderItem> CheckAverageConsumption()
        {
            ModelContext m_context = new ModelContext();

            DateTime now = DateTime.UtcNow;
            DateTime fourWeeksEarlier = now.AddDays(-28);

            List<InvoiceItem> allInvoiceItems = new List<InvoiceItem>();
            List<HistoricalInvoice> invoiceList = m_context.HistoricalInvoices.Where(x => DateTime.Compare(fourWeeksEarlier, x.InvoiceDate) <= 0).ToList();
            foreach (HistoricalInvoice item in invoiceList)
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

            List<DetailOrderItem> detailsToOrder = new List<DetailOrderItem>();

            foreach (InvoiceItem item in allInvoiceItems)
            {
                List<ProdSpecification> prodSpecList = m_context.ProdSpecifications.Where(x => x.ProductId == item.ProductId).ToList();
                foreach (ProdSpecification prodDetail in prodSpecList)
                {
                    if (detailsToOrder.Any(x => x.ItemId == prodDetail.DetailId))
                    {
                        var detailItem = detailsToOrder.Find(x => x.ItemId == prodDetail.DetailId);
                        detailItem.Quantity += (int)(prodDetail.DetailQuantity*item.ProductQuantity);
                    }
                    else
                    {
                        detailsToOrder.Add(new DetailOrderItem()
                        {
                            ItemName = m_context.Details.Where(x => x.DetailId == prodDetail.DetailId).FirstOrDefault().DetailName,
                            ItemId = (int)prodDetail.DetailId,
                            Quantity = (int)(prodDetail.DetailQuantity * item.ProductQuantity)
                        });
                    }
                }
            }

            for (int i = 0; i < detailsToOrder.Count; i++)
            {
                if (detailsToOrder[i].Quantity % 4 != 0)
                {
                    detailsToOrder[i].Quantity = detailsToOrder[i].Quantity / 4 + 1;
                }
                else
                {
                    detailsToOrder[i].Quantity = detailsToOrder[i].Quantity / 4;
                }
            }

            return detailsToOrder;
        }
    }
}
