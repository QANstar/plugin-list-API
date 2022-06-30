using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginList.Model
{
    public class TechnologyEditModel
    {
        public int id { get; set; }
        [StringLength(50)]
        public string technologyName { get; set; }
    }
}
