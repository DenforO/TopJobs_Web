﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TopJobs.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public byte[] Logo { get; set; }
        public string SiteUrl { get; set; }
        public bool Verified { get; set; }
        public ICollection<JobAd> JobAds { get; set; }
        public ICollection<JobExperienceEntry> JobExperienceEntries { get; set; }
        public ICollection<Review> Reviews { get; set; }

    }
}
