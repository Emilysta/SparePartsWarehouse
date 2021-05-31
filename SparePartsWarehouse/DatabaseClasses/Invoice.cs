using System;

#nullable disable

namespace SparePartsWarehouse.DatabaseClasses
{
    public partial class Invoice
    {
        public decimal InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Purchaser { get; set; }
    }
}
