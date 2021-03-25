using System;
using System.Collections.Generic;

#nullable disable

namespace SparePartsWarehouse
{
    public partial class Detail
    {
        public decimal DetailId { get; set; }
        public string DetailName { get; set; }

        public virtual ProdSpecification ProdSpecification { get; set; }
    }
}
