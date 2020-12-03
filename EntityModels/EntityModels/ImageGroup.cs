using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntityModels.EntityModels
{
    public class ImageGroup
    {
        public ImageGroup()
        {

        }
        public int ImageGroupID { get; set; }
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Product Product { get; set; }
        //public int ImageID { get; set; }
        public string ImagePath { get; set; }
    }
}
