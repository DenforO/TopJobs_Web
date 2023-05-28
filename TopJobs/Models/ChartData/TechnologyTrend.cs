using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopJobs.Models.ChartData
{
    public class TechnologyTrend
    {
        public string TechnologyName { get; set; }
        public Dictionary<DateTime, int> MonthlyNumberOfAds { get; set; }
    }
}
