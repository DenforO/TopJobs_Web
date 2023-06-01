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
        public ApplicationUser User { get; set; }
        public DateTime DateApplied { get; set; }
        public string MatchingPercentage { get; set; }
        public CandidateViewModel(ApplicationUser user, DateTime dateApplied, int matchingPercentage)
        {
            User = user;
            DateApplied = dateApplied;
            MatchingPercentage = matchingPercentage + "%";
        }
    }
}
