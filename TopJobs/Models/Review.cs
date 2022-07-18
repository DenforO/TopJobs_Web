using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TopJobs.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        [Required]
        public string Text { get; set; }
        public int PositionTypeId { get; set; }
        public PositionType PositionType { get; set; }
        public TimeSpan? TimePeriod { get; set; }

    }
}
