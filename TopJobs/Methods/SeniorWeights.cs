using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopJobs.Methods
{
    public class SeniorWeights : IWeights
    {
        public double Position => 0.23;

        public double TechStack => 0.23;

        public double Experience => 0.17;

        public double Education => 0.12;

        public double FlexibleSchedule => 0.05;

        public double WorkFromHome => 0.1;

        public double Location => 0.1;

        public double WorkingHours => 0;
    }
}
