using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntityModels.EntityModels
{
    public class Group
    {
        public int GroupID { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        public string GroupName { get; set; }
    }
}
