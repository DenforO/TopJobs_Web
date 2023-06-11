using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TopJobs.Data;
using TopJobs.Models;
using TopJobs.Models.ChartData;

namespace TopJobs.Services
{

    public interface ITrendsService
    {
        List<TechnologyPopularity> GetTechnologyPopularities(int numOfTechnologies);
        List<Dictionary<string, object>> GetTechnologyTrends(DateTime startDate, DateTime endDate, params int[] techIds);
        List<TechnologySelectOption> GetTechnologies();
    }
    public class TrendsService : ITrendsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        public TrendsService(IMemoryCache cache, ApplicationDbContext context)
        {
            _cache = cache;
            _context = context;
        }

        public List<TechnologySelectOption> GetTechnologies()
        {
            return _context.Technologies.Select(t => new TechnologySelectOption { Value = t.Id, Label = t.Name}).ToList();
        }

        public List<TechnologyPopularity> GetTechnologyPopularities(int numOfTechnologies)
        {
            var technologyPopularities = (from t in _context.Technologies
                                          join tp in _context.TechnologyPreferences on t.Id equals tp.TechnologyId
                                          join p in _context.Preferences on tp.PreferenceId equals p.Id
                                          join j in (from j in _context.JobAds where !j.Archived select j) on p.Id equals j.PreferenceId
                                          group t by t.Name into newGroup
                                          orderby newGroup.Count() descending
                                          select new TechnologyPopularity { Name = newGroup.Key, Value = newGroup.Count() }).Take(numOfTechnologies);
            return technologyPopularities.ToList();
        }

        public List<Dictionary<string, object>> GetTechnologyTrends(DateTime startDate, DateTime endDate, params int[] technologyIds)
        {
            //var technology = (from t in _context.Technologies
            //                  where t.Id == technologyId
            //                  join tp in _context.TechnologyPreferences on t.Id equals tp.TechnologyId
            //                  join p in _context.Preferences on tp.PreferenceId equals p.Id
            //                  join j in _context.JobAds on p.Id equals j.PreferenceId
            //                  select new TechnologyTrend { TechnologyName = t.Name, });

            var technologies = _context.Technologies.Where(t => technologyIds.Contains(t.Id));

            var jobAds = _context.TechnologyPreferences
                                        .Where(tp => technologyIds.Contains(tp.TechnologyId))
                                        .Include(tp => tp.Technology)
                                        .Include(tp => tp.Preference)
                                        .ThenInclude(p => p.JobAd)
                                        .Where(tp => tp.Preference.JobAd != null &&
                                                     tp.Preference.JobAd.DateSubmitted > startDate &&
                                                     tp.Preference.JobAd.DateSubmitted < endDate);

            var timePeriod = endDate - startDate;
            var trendData = new List<Dictionary<string, object>>();

            for (DateTime i = startDate; i < endDate; i = i.AddMonths(1))
            {
                //var numberOfAds = jobAds.Where(j => j.Preference.JobAd.DateSubmitted.Month == i.Month && j.Preference.JobAd.DateSubmitted.Year == i.Year).Count();
                var monthEntry = new Dictionary<string, object>();

                monthEntry.Add("Month", i.ToString("MMM yyyy", CultureInfo.InvariantCulture));
                foreach (var technology in technologies)
                {
                    var technologyAds = jobAds.Where(j => j.TechnologyId == technology.Id &&
                                                        j.Preference.JobAd.DateSubmitted.Month == i.Month &&
                                                        j.Preference.JobAd.DateSubmitted.Year == i.Year);

                    monthEntry.Add(technology.Name, technologyAds.Count());

                }
                trendData.Add(monthEntry);
            }
            return trendData;
        }
    }
}
