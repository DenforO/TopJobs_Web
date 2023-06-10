﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // GET: api/<ChartController>
        [HttpGet]
        public IEnumerable<TechnologyPopularity> Get(int? num)
        {
            if (!num.HasValue)
            {
                num = 5;
            }
            return _trendsService.GetTechnologyPopularities(num.Value);
        }

        // GET: api/<ChartController>
        [HttpGet]
        [Route("TechnologyTrend")]
        public IEnumerable<Dictionary<string, object>> GetTechnologyTrend([FromQuery] int[] techId)
        {
            return _trendsService.GetTechnologyTrends(new DateTime(2021, 05, 10), DateTime.Now, techId);
        }

        // GET api/<ChartController>/5
        [HttpGet("{id}")]
        public IEnumerable<TechnologyPopularity> Get(int num)
        {
            return _trendsService.GetTechnologyPopularities(num);
        }

        // POST api/<ChartController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ChartController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ChartController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
