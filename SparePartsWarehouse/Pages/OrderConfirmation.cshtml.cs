using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SparePartsWarehouse;

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
            Request.Form.TryGetValue("Purchaser", out Microsoft.Extensions.Primitives.StringValues purchaser);
            Request.Form.TryGetValue("Product", out Microsoft.Extensions.Primitives.StringValues products);
            Request.Form.TryGetValue("Quantity", out Microsoft.Extensions.Primitives.StringValues quantities);
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
