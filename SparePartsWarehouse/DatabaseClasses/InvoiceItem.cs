#nullable disable

namespace SparePartsWarehouse.DatabaseClasses
{
    public partial class InvoiceItem
    {
        public decimal InvoiceId { get; set; }
        public decimal ProductId { get; set; }
        public decimal ProductQuantity { get; set; }
        public decimal Key { get; set; }
        public decimal InvoinceItemNumber { get; set; }
        public Product Product { get; internal set; }
    }
}
