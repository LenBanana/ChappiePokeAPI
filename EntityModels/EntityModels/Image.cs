using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntityModels.EntityModels
{
    public class Image
    {
        public int ImageID { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Thumbnail { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Fullsize { get; set; }
    }
}
