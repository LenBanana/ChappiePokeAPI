using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntityModels.EntityModels
{
    public class Card
    {
        public int CardID { get; set; }
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
        public int ImageID { get; set; }
        public virtual Image Image { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
