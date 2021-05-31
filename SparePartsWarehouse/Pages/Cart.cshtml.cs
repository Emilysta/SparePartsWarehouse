using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace SparePartsWarehouse.Pages
{
    public class CartItem
    {
        public string ProductName;
        public int ProductId;
        public int Count;
    }
    public class CartModel : PageModel
    {
        public List<CartItem> OrderItems { get; set; }
        public void OnGet()
        {
            OrderItems = new List<CartItem>();
            if (Request.Cookies.TryGetValue("CartList", out string cartListString))
                OrderItems = JsonConvert.DeserializeObject<List<CartItem>>(cartListString);
        }

        public async void OnPostAsync()
        {
            OrderItems = new List<CartItem>();
            if (Request.Cookies.TryGetValue("CartList", out string cartListString))
            {
                OrderItems = JsonConvert.DeserializeObject<List<CartItem>>(cartListString);
                Response.Cookies.Delete("CartList");

                if (Request.Form.TryGetValue("productID", out var productID))
                    OrderItems.Remove(OrderItems.Find(x => x.ProductId == int.Parse(productID)));

                int itemsCount = 0;
                foreach (CartItem x in OrderItems)
                    itemsCount += x.Count;

                Response.Cookies.Append("CartList", JsonConvert.SerializeObject(OrderItems));
                Response.Cookies.Append("CartItemsCount", itemsCount.ToString());
                if (itemsCount == 0)
                    Response.Cookies.Delete("CartItemsCount");
            }
        }
    }
}
