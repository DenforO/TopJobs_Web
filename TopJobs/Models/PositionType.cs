﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TopJobs.Models
{
    public class PositionType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Level { get; set; }
        public ICollection<JobExperienceEntry> JobExperienceEntries { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Preference> Preferences { get; set; }
    }
}
