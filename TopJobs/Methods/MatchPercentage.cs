using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using TopJobs.Enums;
using TopJobs.Models;

namespace TopJobs.Methods
{
    public static class MatchPercentage
    {
        public static int Calculate(ApplicationUser user, JobAd jobAd, string level)
        {
            IWeights weights;

            switch (level)
            {
                case "Intern":
                    weights = new InternWeights();
                    break;
                case "Junior":
                    weights = new JuniorWeights();
                    break;
                case "Senior":
                    weights = new SeniorWeights();
                    break;
                case "Mid":
                default:
                    weights = new MidWeights();
                    break;
            }

            double result = 0;
            result += weights.Position * PositionMatch(user.Preference.PositionType, jobAd.Preference.PositionType);
            result += weights.TechStack * TechStackMatch(user.Preference.TechnologyPreferences, jobAd.Preference.TechnologyPreferences);
            result += weights.Experience * ExperienceMatch(GetUserExperience(user), Convert.ToInt32(jobAd.RequiredExperience));
            result += weights.Education * EducationMatch(user.EducationEntries);
            result += weights.FlexibleSchedule * PreferenceMatch(user.Preference.FlexibleSchedule ?? false, jobAd.Preference.FlexibleSchedule ?? false);
            result += weights.WorkFromHome * PreferenceMatch(user.Preference.WorkFromHome ?? false, jobAd.Preference.WorkFromHome ?? false);
            result += weights.Location * LocationMatch(user.City, jobAd.Company.City, jobAd.Preference.WorkFromHome ?? true);
            result += weights.WorkingHours * WorkingHoursMatch(user.Preference.WorkingHours ?? 8, jobAd.Preference.WorkingHours ?? 8);
            return Convert.ToInt32(result * 100);
        }
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

        private static double PositionMatch(PositionType userPositionType, PositionType adPositionType)
        {
            double result = 0;

            if (userPositionType.Name.Equals(adPositionType.Name))
            {
                result += 0.7;
            }
            if (userPositionType.Level.Equals(adPositionType.Level) || adPositionType.Level == null || userPositionType == null)
            {
                result += 0.3;
            }

            return result;
        }

        private static double TechStackMatch(ICollection<TechnologyPreference> userTechStack, ICollection<TechnologyPreference> adTechStack)
        {
            var matchinTechnolgoies = userTechStack
                                            .Join(adTechStack, 
                                                  u => u.TechnologyId,
                                                  a => a.TechnologyId, 
                                                  (u, a) => u);
            if (matchinTechnolgoies.Count() == 0)
            {
                return 0;
            }
            return (float)matchinTechnolgoies.Count() / adTechStack.Count;
        }

        private static double ExperienceMatch(int userExperience, int adExperience)
        {
            if (userExperience < adExperience)
            {
                return 0;
            }

            if (adExperience == 0)
            {
                adExperience += 1;
            }
            float difference = userExperience - adExperience;
            if (difference < adExperience / 2)
            {
                return 1;
            }

            var result = 1 - (difference / adExperience - 0.5) * 2; // loses percentage if overqualififed
            return Math.Max(result, 0);
        }

        private static double EducationMatch(ICollection<EducationEntry> userEducationEntries)
        {
            double result = 0;
            foreach (var entry in userEducationEntries)
            {
                switch (entry.EducationType.Name)
                {
                    case "Bachelor's degree":
                    case "Master's degree":
                        result += 0.1; 
                        if (entry.DateFinished.HasValue)
                        {
                            result += 0.2;
                        }
                        break;
                    case "Course":
                    case "Bootcamp":
                        result += 0.03;
                        break;
                    case "Academy":
                        result += 0.03 * NumberOfMonthsBetween(entry.DateFinished ?? DateTime.Today, entry.DateStarted);
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
        private static double PreferenceMatch(bool userRequires, bool adOffers)
        {
            // For flexible schedule and work from home

            return Convert.ToDouble(!userRequires | adOffers);
        }
        private static double LocationMatch(string userCity, string adCity, bool adOffersWorkFromHome)
        {
            if (adOffersWorkFromHome)
            {
                return 1;
            }
            if (string.IsNullOrEmpty(userCity))
            {
                return 0.5;
            }
            return Convert.ToDouble(userCity.Equals(adCity) || adOffersWorkFromHome);
        }
        private static double WorkingHoursMatch(int userHours, int adHours)
        {
            return Math.Max(1 - Math.Abs(userHours - adHours) * 0.25, 0);
        }
        private static int GetUserExperience(ApplicationUser user)
        {
            int experienceInMonths = 0;

            foreach (var entry in user.JobExperienceEntries)
            {
                var startDate = entry.DateStarted;
                var endDate = entry.DateFinished ?? DateTime.Today;

                experienceInMonths += NumberOfMonthsBetween(endDate, startDate);
            }

            var experienceInYears = experienceInMonths / 12 + (experienceInMonths % 12) / 6;
            return experienceInYears;
        }
        private static int NumberOfMonthsBetween(DateTime endDate, DateTime startDate)
        {
            if (startDate > endDate) // in case of mistake in input
            {
                var temp = endDate;
                endDate = startDate;
                startDate = temp;
            }

            return ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month;
        }
    }
}
