using System;
using System.Collections.Generic;

#nullable disable

namespace SparePartsWarehouse
{
    public partial class ProdSpecification
    {
        public decimal DetailId { get; set; }
        public decimal ProductId { get; set; }
        public decimal DetailQuantity { get; set; }

        public virtual Detail Detail { get; set; }
        public virtual Product Product { get; set; }
    }
}
