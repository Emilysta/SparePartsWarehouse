using System;
using System.Collections.Generic;

#nullable disable

namespace SparePartsWarehouse.DatabaseClasses
{
    public partial class Stock
    {
        public decimal DetailId { get; set; }
        public string DetailName { get; set; }
        public decimal Quantity { get; set; }
    }
}
