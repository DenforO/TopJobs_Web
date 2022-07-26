using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TopJobs.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UsernameChangeLimit { get; set; } = 10;
        public byte[] ProfilePicture { get; set; }
        public string City { get; set; }
        public int PreferenceId { get; set; }
        public Preference Preference { get; set; }
        public ICollection<JobApplication> JobApplications { get; set; }
        public ICollection<EducationEntry> EducationEntries { get; set; }
        public ICollection<JobExperienceEntry> JobExperienceEntries { get; set; }
    }
}
