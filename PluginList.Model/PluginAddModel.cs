using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginList.Model
{
    public class PluginAddModel
    {
        [Required]
        [StringLength(50)]
        public string pluginName { get; set; }
        [StringLength(500)]
        public string introduce { get; set; }
        [StringLength(500)]
        public string instruction { get; set; }
        [StringLength(500)]
        public string webUrl { get; set; }
        public int parTechnologyId { get; set; }
    }
}
