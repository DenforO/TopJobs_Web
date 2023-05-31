using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopJobs.Models;

namespace TopJobs.ViewModels
{
    public class EmployerJobAdsViewModel
    {
        public string Name { get; set; }
        public Company Company { get; set; }
        public DateTime DateSubmitted { get; set; }
        public int NumberOfCandidates { get; set; }
    }
}
