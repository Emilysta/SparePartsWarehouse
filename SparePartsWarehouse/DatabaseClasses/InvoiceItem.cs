using System;
using System.Collections.Generic;

#nullable disable

namespace SparePartsWarehouse
{
    public partial class InvoiceItem
    {
        public decimal InvoiceId { get; set; }
        public decimal ProductId { get; set; }
        public decimal ProductQuantity { get; set; }
        public decimal Key { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual Product Product { get; set; }
    }
}
