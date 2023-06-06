using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopJobs.Methods;
using TopJobs.Models;

namespace TopJobs.ViewModels
{
    public class CandidateViewModel
    {
        public JobApplication JobApplication { get; set; }
        public string Match { get; set; }
        public CandidateViewModel(JobApplication jobApplication)
        {
            JobApplication = jobApplication;
            Match = MatchPercentage.CalculateMatchPercentage(jobApplication.User.Preference, jobApplication.JobAd.Preference) + "%";
        }
    }
}
