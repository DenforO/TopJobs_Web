﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TopJobs.Data;
using TopJobs.Models.ChartData;

namespace TopJobs.Services
{

    public interface ITrendsService
    {
        List<TechnologyPopularity> GetTechnologyPopularities(int numOfTechnologies);
        List<TechnologyTrend> GetTechnologyTrend(int technologyId, DateTime startDate, DateTime endDate);
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
        public List<TechnologyPopularity> GetTechnologyPopularities(int numOfTechnologies)
        {
            var technologyPopularities = (from t in _context.Technologies
                                         join tp in _context.TechnologyPreferences on t.Id equals tp.TechnologyId
                                         join p in _context.Preferences on tp.PreferenceId equals p.Id
                                         join j in _context.JobAds on p.Id equals j.PreferenceId
                                         group t by t.Name into newGroup
                                         orderby newGroup.Count() descending
                                         select new TechnologyPopularity { Name = newGroup.Key, Value = newGroup.Count() }).Take(numOfTechnologies);
            return technologyPopularities.ToList();
        }

        public List<TechnologyTrend> GetTechnologyTrend(int technologyId, DateTime startDate, DateTime endDate)
        {
            //var technology = (from t in _context.Technologies
            //                  where t.Id == technologyId
            //                  join tp in _context.TechnologyPreferences on t.Id equals tp.TechnologyId
            //                  join p in _context.Preferences on tp.PreferenceId equals p.Id
            //                  join j in _context.JobAds on p.Id equals j.PreferenceId
            //                  select new TechnologyTrend { TechnologyName = t.Name, });

            var jobAds = _context.TechnologyPreferences
                                    .Where(tp => tp.TechnologyId == technologyId)
                                    .Include(tp => tp.Technology)
                                    .Include(tp => tp.Preference)
                                    .ThenInclude(p => p.JobAd)
                                    .Where(tp => tp.Preference.JobAd != null &&
                                                 tp.Preference.JobAd.DateSubmitted > startDate &&
                                                 tp.Preference.JobAd.DateSubmitted < endDate)
                                    .Select(tp => tp.Preference.JobAd);

            var timePeriod = endDate - startDate;
            var trendData = new List<TechnologyTrend>();
            for (DateTime i = startDate; i < endDate; i = i.AddMonths(1))
            {
                var numberOfAds = jobAds.Where(j => j.DateSubmitted.Month == i.Month && j.DateSubmitted.Year == i.Year).Count();
                trendData.Add(
                    new TechnologyTrend
                    {
                        Month = i.ToString("MMM yy", CultureInfo.InvariantCulture),
                        Ads = numberOfAds 
                    });
            }
            return trendData;
        }
    }
}
