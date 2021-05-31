using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SparePartsWarehouse.Pages
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
            HttpContext.Session.Clear();
        }
    }
}
