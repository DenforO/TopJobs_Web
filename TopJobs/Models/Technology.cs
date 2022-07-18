using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TopJobs.Models
{
    public class Technology
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<TechnologyPreference> TechnologyPreferences { get; set; }
    }
}
