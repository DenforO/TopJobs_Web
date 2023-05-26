using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopJobs.Data;
using TopJobs.Models.ChartData;

namespace TopJobs.Services
{

    public interface ITrendsService
    {
        List<TechnologyPopularity> GetTechnologyPopularities(int numOfTechnologies);
        List<TechnologyPopularity> GetTechnologyTrend(DateTime startDate, DateTime endDate);
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

        public List<TechnologyPopularity> GetTechnologyTrend(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
