using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopJobs.Models
{
    public class JobAd
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public DateTime dateSubmitted { get; set; }
        public int PreferenceId { get; set; }
        public TimeSpan requiredExperience { get; set; }
        public ICollection<JobApplication> JobApplications { get; set; }

    }
}
