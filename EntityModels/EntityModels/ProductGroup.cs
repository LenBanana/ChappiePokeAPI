using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntityModels.EntityModels
{
    public class ProductGroup
    {
        public ProductGroup()
        {

        }
        public int ProductGroupID { get; set; }
        public int ProductID { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
        //public int GroupID { get; set; }
        //[ForeignKey("GroupID")]
        public string GroupName { get; set; }
    }
}
