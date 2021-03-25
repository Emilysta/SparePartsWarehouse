using System;
using System.Collections.Generic;

#nullable disable

namespace SparePartsWarehouse
{
    public partial class Product
    {
        public Product()
        {
            ProdSpecifications = new HashSet<ProdSpecification>();
        }
        public decimal ProductId { get; set; }
        public string ProductName { get; set; }

        public virtual ICollection<ProdSpecification> ProdSpecifications { get; set; }
    }
}
