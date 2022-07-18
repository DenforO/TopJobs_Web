using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TopJobs.Models
{
    public class EducationEntry
    {
        public int Id { get; set; }
        [Required]
        public string School { get; set; }
        [Required]
        public string Description { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int EducationTypeId { get; set; }
        public EducationType EducationType { get; set; }
        [Required]
        public DateTime DateStarted { get; set; }
        public DateTime? DateFinished { get; set; }
    }
}
