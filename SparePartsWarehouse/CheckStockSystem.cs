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
        public static void Run()
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
    }
}
