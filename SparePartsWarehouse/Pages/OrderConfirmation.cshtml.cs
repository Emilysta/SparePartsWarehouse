using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SparePartsWarehouse.Pages
{
    public class OrderConfirmationModel : PageModel
    {
        public string Message { get; set; }
        public void OnGet()
        {
        }
        public void OnPost()
        {
            Message = "Post Used";
            //Request.Form[]
            foreach(string s in Request.Query.Keys)
            {
                Message += s;
            }
        }
    }
}
