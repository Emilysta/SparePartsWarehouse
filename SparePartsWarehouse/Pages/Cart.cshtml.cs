using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public List<CartItem> orderItems { get; set; }
        public void OnGet()
        {
            orderItems = new List<CartItem>();
            if (Request.Cookies.TryGetValue("CartList", out string cartListString))
                orderItems = JsonConvert.DeserializeObject<List<CartItem>>(cartListString);
        }

        public async void OnPostAsync()
        {
            orderItems = new List<CartItem>();
            if (Request.Cookies.TryGetValue("CartList", out string cartListString))
            {
                orderItems = JsonConvert.DeserializeObject<List<CartItem>>(cartListString);
                Response.Cookies.Delete("CartList");

                if (Request.Form.TryGetValue("productID", out var productID))
                    orderItems.Remove(orderItems.Find(x => x.ProductId == int.Parse(productID)));

                int itemsCount = 0;
                foreach (CartItem x in orderItems)
                    itemsCount += x.Count;

                Response.Cookies.Append("CartList", JsonConvert.SerializeObject(orderItems));
                Response.Cookies.Append("CartItemsCount", itemsCount.ToString());
                if (itemsCount == 0)
                    Response.Cookies.Delete("CartItemsCount");
            }
        }
    }
}
