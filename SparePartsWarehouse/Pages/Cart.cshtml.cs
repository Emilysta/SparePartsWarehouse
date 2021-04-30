using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SparePartsWarehouse.Pages
{
    public class CartModel : PageModel
    {
        public List<OrderItem> orderItems { get; set; }
        public void OnGet()
        {
            orderItems = new List<OrderItem>();
        }
    }
}
