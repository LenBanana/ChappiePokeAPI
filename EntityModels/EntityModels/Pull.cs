using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntityModels.EntityModels
{
    public class Pull
    {
        public Pull()
        {

        }
        public int PullID { get; set; }
        public int CardID { get; set; }
        [ForeignKey("CardID")]
        public virtual Card Card { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        public string PulledBy { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }
        public DateTime LastModified { get; set; }
    }
}
