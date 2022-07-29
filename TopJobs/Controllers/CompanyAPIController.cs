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
            return _context.Companies.Where(x => x.Name.StartsWith(name) || string.IsNullOrEmpty(name)).Include(x => x.JobAds).ToList();
        }
    }
}
