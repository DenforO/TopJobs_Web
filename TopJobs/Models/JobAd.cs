using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TopJobs.Models
{
    public class JobAd
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public DateTime DateSubmitted { get; set; }
        public int PreferenceId { get; set; }
        public Preference Preference { get; set; }
        public float RequiredExperience { get; set; }
        public ICollection<JobApplication> JobApplications { get; set; }

        public bool Archived { get; set; }

        [NotMapped]
        public int MatchingPercentage { set; get; }

    }
}
