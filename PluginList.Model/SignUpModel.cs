using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QANbuy.Model
{
    public class SignUpModel
    {
        [Required]
        [StringLength(50)]
        public string userName { get; set; }
        [Required]
        [StringLength(50)]
        public string password { get; set; }
        [Required]
        [StringLength(50)]
        public string email { get; set; }
    }
}
