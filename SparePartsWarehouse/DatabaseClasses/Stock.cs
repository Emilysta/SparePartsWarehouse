using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SparePartsWarehouse.DatabaseClasses
{
    public partial class Stock
    {
        public decimal DetailId { get; set; }
        public string DetailName { get; set; }
        [ConcurrencyCheck]
        public decimal Quantity { get; set; }
    }
}
