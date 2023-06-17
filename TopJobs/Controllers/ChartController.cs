using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopJobs.Models;
using TopJobs.Models.ChartData;
using TopJobs.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TopJobs.Controllers
{
    [Route("api/Trends")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly ITrendsService _trendsService;
        public ChartController(ITrendsService trendsService)
        {
            _trendsService = trendsService;
        }

        // GET: api/trends
        [HttpGet]
        public IEnumerable<TechnologyPopularity> Get(int? num)
        {
            if (!num.HasValue)
            {
                num = 5;
            }
            return _trendsService.GetTechnologyPopularities(num.Value);
        }

        // GET: api/trends/technologytrend?techid=1&techid=4
        [HttpGet]
        [Route("TechnologyTrend")]
        public IEnumerable<Dictionary<string, object>> GetTechnologyTrend([FromQuery] int[] techId)
        {
            return _trendsService.GetTechnologyTrends(new DateTime(2021, 05, 10), DateTime.Now, techId);
        }

        // GET: api/trends/technologies
        [HttpGet]
        [Route("Technologies")]
        public List<TechnologySelectOption> GetTechnologies()
        {
            return _trendsService.GetTechnologies();
        }

        // GET api/<ChartController>/5
        [HttpGet("{id}")]
        public IEnumerable<TechnologyPopularity> Get(int num)
        {
            return _trendsService.GetTechnologyPopularities(num);
        }
    }
}
