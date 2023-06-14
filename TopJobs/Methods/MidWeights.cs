using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopJobs.Methods
{
    public class MidWeights : IWeights
    {
        public double Position => 0.2;

        public double TechStack => 0.2;

        public double Experience => 0.15;

        public double Education => 0.1;

        public double FlexibleSchedule => 0.1;

        public double WorkFromHome => 0.1;

        public double Location => 0.1;

        public double WorkingHours => 0.05;
    }
}
