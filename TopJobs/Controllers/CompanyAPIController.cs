using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TopJobs.Data;
using TopJobs.Models;
using System.Data;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TopJobs.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CompanyAPIController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public CompanyAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<CompanyAPIController>
        [Route("GetCompanies")]
        [HttpGet]
        public IEnumerable<Company> GetCompanies(string name)
        {
            return _context.Companies
                                .Where(x => x.Name.StartsWith(name) || string.IsNullOrEmpty(name))
                                .Include(x => x.JobAds)
                                .ThenInclude(j => j.Preference)
                                .ThenInclude(p => p.PositionType)
                                .ToList();
        }

        [Route("GetCompaniesPrefix")]
        [HttpPost]
        public IEnumerable<object> GetCompaniesPrefix([FromBody]string prefix)
        {
            //Note : you can bind same list from database  
            List<Company> allCompanies = _context.Companies.ToList();

            var results = allCompanies.Where(x => x.Name.StartsWith(prefix)).Select(x => new { Name = x.Name, Id = x.Id }).ToList();

            return results;
        }
    }
}
