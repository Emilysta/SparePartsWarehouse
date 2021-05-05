using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;

namespace SparePartsWarehouse.Pages
{

    public class OrderConfirmationModel : PageModel
    {
        public List<CartItem> ProductsList { get; set; }
        public string Purchaser { get; set; }
        public void OnPost()
        {
            Request.Form.TryGetValue("Purchaser", out StringValues purchaser);
            Request.Form.TryGetValue("ProductID", out StringValues productIDs);
            Request.Form.TryGetValue("ProductName", out StringValues productNames);
            Request.Form.TryGetValue("Quantity", out StringValues quantities);
            ProductsList = new List<CartItem>();
            int i = 0;
            foreach (string s in productIDs)
            {
                ProductsList.Add(new CartItem
                {
                    ProductId = int.Parse(productIDs[i]),
                    ProductName = productNames[i],
                    Count = int.Parse(quantities[i])
                });
                i++;
            }
            Purchaser = purchaser;

            Response.Cookies.Delete("CartList");
            Response.Cookies.Delete("CartItemsCount");

            OrderSystem.MakeOrder(purchaser[0], ProductsList);
        }
    }
}
