using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntityModels.EntityModels
{
    public class Customer
    {
        public Customer()
        {
            Orders = new List<Order>();
            User = new User();
            BillingAddress = new Address();
            ShippingAddress = new Address();
        }

        public int CustomerID { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        public string Name { get; set; }
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        public string Lastname { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        public string CompanyName { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        public string ContactPerson { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        public string Phone { get; set; }
        public DateTime LastModified { get; set; }
        public int BillingAddressAddressID { get; set; }
        [ForeignKey("BillingAddressAddressID")]
        public virtual Address BillingAddress { get; set; }
        public int ShippingAddressAddressID { get; set; }
        [ForeignKey("ShippingAddressAddressID")]
        public virtual Address ShippingAddress { get; set; }
        public virtual List<Order> Orders { get; set; }
    }
}
