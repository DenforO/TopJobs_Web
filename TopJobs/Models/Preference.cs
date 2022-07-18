using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopJobs.Models
{
    public class Preference
    {
        public int Id { get; set; }
        public int PositionTypeId { get; set; }
        public PositionType PositionType { get; set; }
        public int? WorkingHours { get; set; }
        public bool? FlexibleSchedule { get; set; }
        public bool? WorkFromHome { get; set; }
        public ICollection<TechnologyPreference> TechnologyPreferences { get; set; }
    }
}
