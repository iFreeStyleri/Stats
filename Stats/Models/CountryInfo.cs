using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stats.Models
{
    public class CountryInfo
    { 
        public string Name { get; set; }
        public string Province { get; set; }
        public IEnumerable<ConfirmedCount> Counts { get; set; }
    }
}
