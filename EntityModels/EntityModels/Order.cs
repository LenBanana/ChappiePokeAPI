using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntityModels.EntityModels
{
    public class Order
    {
        public Order()
        {

        }
        public int OrderID { get; set; }
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual User User { get; set; }
        public int CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Customer Customer { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateOrder { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Status { get; set; }
        public DateTime LastModified { get; set; }
        public virtual ICollection<SaleOrderProduct> SaleOrderProducts { get; set; }
    }
}
