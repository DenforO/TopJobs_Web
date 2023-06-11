using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopJobs.Models;

namespace TopJobs.Methods
{
    public static class MatchPercentage
    {
        public static int CalculateMatchPercentage(Preference userPreference, Preference jobAdPreference)
        {
            int result = 0;

            if (userPreference.PositionType.Level == null || jobAdPreference.PositionType.Level == null || userPreference.PositionType.Level.Equals(jobAdPreference.PositionType.Level))
            {
                result += 30;
            }
            else if (
                (userPreference.PositionType.Level.Equals("Junior") && jobAdPreference.PositionType.Level.Equals("Mid")) ||
                (userPreference.PositionType.Level.Equals("Mid") && jobAdPreference.PositionType.Level.Equals("Senior")) ||
                (userPreference.PositionType.Level.Equals("Senior") && jobAdPreference.PositionType.Level.Equals("Mid")) ||
                (userPreference.PositionType.Level.Equals("Mid") && jobAdPreference.PositionType.Level.Equals("Junior")))
                   
            {
                result += 15;
            }

            if (userPreference.PositionType.Name.Equals(jobAdPreference.PositionType.Name))
            {
                result += 30;
            }

            int matchingTechnologies = 0;

            foreach (var jobAdTechnology in jobAdPreference.TechnologyPreferences)
            {
                foreach (var userTechnology in userPreference.TechnologyPreferences)
                {
                    if (jobAdTechnology.TechnologyId == userTechnology.TechnologyId)
                    {
                        matchingTechnologies++;
                        break;
                    }
                }
            }

            float ratio = (float)matchingTechnologies / Math.Max((float)jobAdPreference.TechnologyPreferences.Count, 1);
            result += Convert.ToInt32(ratio * 20);

            if (jobAdPreference.WorkingHours == null || userPreference.WorkingHours == null || userPreference.WorkingHours == jobAdPreference.WorkingHours)
            {
                result += 10;
            }
            if (jobAdPreference.FlexibleSchedule == null || userPreference.FlexibleSchedule == null || userPreference.FlexibleSchedule == jobAdPreference.FlexibleSchedule)
            {
                result += 5;
            }
            if (jobAdPreference.WorkFromHome == null || userPreference.WorkFromHome == null || userPreference.WorkFromHome == jobAdPreference.WorkFromHome)
            {
                result += 5;
            }
            

            return result;
        }
    }
}
