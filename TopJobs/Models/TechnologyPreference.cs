using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopJobs.Models
{
    public class TechnologyPreference
    {
        public int TechnologyId { get; set; }
        public Technology Technology { get; set; }
        public int PreferenceId { get; set; }
        public Preference Preference { get; set; }
    }
}
