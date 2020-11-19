using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntityModels.EntityModels
{
    public class Address
    {
        public Address()
        {
            AddressID = 0;
        }
        public int AddressID { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        public string AddressLine1 { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        public string AddressLine2 { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        public string City { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        public string State { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        public string Country { get; set; }

        [Required(AllowEmptyStrings = true)]
        public int PostalCode { get; set; }
        public DateTime LastModified { get; set; }
    }
}
