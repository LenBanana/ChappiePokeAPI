using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntityModels.EntityModels
{
    public class SaleOrderProduct
    {
        public SaleOrderProduct()
        {

        }
        public int SaleOrderProductID { get; set; }
        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Order Order { get; set; }
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
        public double Cost { get; set; }
        public double Price { get; set; }
    }
}
