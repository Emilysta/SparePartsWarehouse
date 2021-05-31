using System;
using System.Collections.Generic;

#nullable disable

namespace SparePartsWarehouse.DatabaseClasses
{
    public partial class HistoricalInvoice
    {
        public decimal InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Purchaser { get; set; }
    }
}
