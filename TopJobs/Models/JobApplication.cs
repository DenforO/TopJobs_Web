using System;
using System.Collections.Generic;
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
        public DateTime DateApplied { get; set; }
    }
}
