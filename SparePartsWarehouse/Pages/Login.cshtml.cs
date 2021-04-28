using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace SparePartsWarehouse.Pages
{
    public class LoginModel : PageModel
    {
        public IActionResult OnPost()
        {
            Request.Form.TryGetValue("username", out Microsoft.Extensions.Primitives.StringValues username);
            Request.Form.TryGetValue("password", out Microsoft.Extensions.Primitives.StringValues password);
            if(username == "admin" && password == "admin")
            {
                HttpContext.Session.SetString("user", "admin");
                return Redirect("/Index");
            }
            return Redirect("/Index");
        }
        public void OnGet()
        {
        }
    }
}
