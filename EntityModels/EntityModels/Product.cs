using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntityModels.EntityModels
{
    public class Product
    {
        public Product()
        {

        }
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
        public virtual ICollection<ProductGroup> ProductGroups { get; set; }
        public virtual ICollection<ImageGroup> ImageGroups { get; set; }
        public virtual ICollection<Card> Cards { get; set; }
        public virtual ICollection<SaleOrderProduct> SaleOrderProducts { get; set; }

    }
}
