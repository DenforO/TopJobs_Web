using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopJobs.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public byte[] Logo { get; set; }
        public string SiteUrl { get; set; }
        public bool Verified { get; set; }
        public ICollection<JobAd> JobAds { get; set; }
    }
}
