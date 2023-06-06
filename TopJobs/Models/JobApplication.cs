using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TopJobs.Models
{
    public class JobApplication
    {
        public int JobAdId { get; set; }
        public JobAd JobAd { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public DateTime DateApplied { get; set; }
        public int MatchingPercentage { get; set; }
        public bool Accepted { get; set; }
    }
}
