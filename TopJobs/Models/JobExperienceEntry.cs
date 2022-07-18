using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopJobs.Models
{
    public class JobExperienceEntry
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime? DateFinished { get; set; }
        public int PositionTypeId { get; set; }
        public PositionType PositionType { get; set; }
        public bool Verified { get; set; }
    }
}
