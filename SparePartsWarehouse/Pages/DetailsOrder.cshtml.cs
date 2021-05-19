using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SparePartsWarehouse.Pages
{
    public class DetailsOrderModel : PageModel
    {
        private readonly ModelContext _context;

        [Required]
        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Ilość może zawierać tylko liczby większe od 0")]
        public int Quantity { get; set; }
        
        [BindProperty(SupportsGet =true)]
        public List<DetailOrderItem> detailsList { get; set; }

        [Required]
        [Display(Name = "Region")]
        public string SelectedId { get; set; }

        public IEnumerable<SelectListItem> DetailsNames { get; set; }

        public DetailsOrderModel(ModelContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
           DetailsNames = GetDetails();
            _context.Dispose();
        }

        public void OnPost()
        {
            Request.Form.TryGetValue("Detail", out Microsoft.Extensions.Primitives.StringValues details);
            Request.Form.TryGetValue("Quantity", out Microsoft.Extensions.Primitives.StringValues quantities);
            detailsList = new List<DetailOrderItem>();
            int i = 0;
            foreach (string s in details)
            {
                detailsList.Add(new DetailOrderItem
                {
                    ItemName = details[i],
                    Quantity = int.Parse(quantities[i])
                });
                i++;
            }
            DetailOrderSystem.MakeDetailOrder(detailsList);
        }

        public IEnumerable<SelectListItem> GetDetails()
        {
            IEnumerable<SelectListItem> detailsList = _context.Details.Select(n =>
        new SelectListItem
        {
            Value = n.DetailId.ToString(),
            Text = n.DetailName
        }).ToList();
            return new SelectList(detailsList, "Value", "Text");
        }
    }
}
