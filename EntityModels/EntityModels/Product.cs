using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntityModels.EntityModels
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        public string Type { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }
        public double Cost { get; set; }
        public double Price { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual List<ProductGroup> ProductGroups { get; set; }
        public virtual List<ImageGroup> ImageGroups { get; set; }
        public virtual List<Card> Cards { get; set; }
        public virtual List<SaleOrderProduct> SaleOrderProducts { get; set; }

    }
}
