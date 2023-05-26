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
            //var technologyPopularities = _context.
        }

        public List<TechnologyPopularity> GetTechnologyTrend(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
