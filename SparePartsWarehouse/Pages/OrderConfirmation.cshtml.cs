using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;

namespace SparePartsWarehouse.Pages
{
    public struct OrderItem
    {
        public string ItemName;
        public int Quantity;
    }
    public class OrderConfirmationModel : PageModel
    {
        public List<OrderItem> ProductsList { get; set; }
        public string Purchaser { get; set; }
        public void OnPost()
        {
            Request.Form.TryGetValue("Purchaser", out StringValues purchaser);
            Request.Form.TryGetValue("Product", out StringValues products);
            Request.Form.TryGetValue("Quantity", out StringValues quantities);
            ProductsList = new List<OrderItem>();
            int i = 0;
            foreach (string s in products)
            {
                ProductsList.Add(new OrderItem
                {
                    ItemName = products[i],
                    Quantity = int.Parse(quantities[i])
                });
                i++;
            }
            OrderSystem.MakeOrder(purchaser[0], ProductsList);
        }
    }
}
