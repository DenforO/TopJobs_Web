using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopJobs.Methods
{
    public interface IWeights
    {
        double Position { get; }
        double TechStack { get; }
        double Experience { get; }
        double Education { get; }
        double FlexibleSchedule { get; }
        double WorkFromHome { get; }
        double Location { get; }
        double WorkingHours { get; }
    }
}
