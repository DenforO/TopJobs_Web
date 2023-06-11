using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopJobs.Models;

namespace TopJobs.ViewModels
{
    public class JobExperienceViewModel
    {
        public string Company { get; protected set; }
        public string Position { get; protected set; }
        public string Timeframe { get; protected set; }
        public bool Verified { get; protected set; }
        public JobExperienceViewModel(JobExperienceEntry jobExperienceEntry)
        {
            Company = jobExperienceEntry.Company.Name;

            if(jobExperienceEntry.PositionType.Level.Equals("-"))
            {
                Position = jobExperienceEntry.PositionType.Name;
            }
            else
            {
                Position = jobExperienceEntry.PositionType.Level + " " + jobExperienceEntry.PositionType.Name;
            }

            Verified = jobExperienceEntry.Verified;

            if (jobExperienceEntry.DateFinished.HasValue)
            {
                Timeframe = jobExperienceEntry.DateStarted.ToShortDateString() + " - " + jobExperienceEntry.DateFinished.Value.ToShortDateString();
            }
            else
            {
                Timeframe = jobExperienceEntry.DateStarted.ToShortDateString() + " - Present";
            }
        }
    }
}
