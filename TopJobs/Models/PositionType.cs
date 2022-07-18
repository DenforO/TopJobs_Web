using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopJobs.Models
{
    public class PositionType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
        public ICollection<JobExperienceEntry> JobExperienceEntries { get; set; }
    }
}
