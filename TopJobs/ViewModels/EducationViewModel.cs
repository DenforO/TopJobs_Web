using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopJobs.Models;

namespace TopJobs.ViewModels
{
    public class EducationViewModel
    {
        public string School { get; protected set; }
        public string EducationType { get; protected set; }
        public string Description { get; protected set; }
        public string Timeframe { get; protected set; }
        public EducationViewModel(EducationEntry entry)
        {
            School = entry.School;
            EducationType = entry.EducationType.Name;
            Description = entry.Description;

            if (entry.DateFinished.HasValue)
            {
                Timeframe = entry.DateStarted.ToShortDateString() + " - " + entry.DateFinished.Value.ToShortDateString();
            }
            else
            {
                Timeframe = entry.DateStarted.ToShortDateString() + " - Present";
            }
        }
    }
}
