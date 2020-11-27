using System;

namespace OnTheBeachExercise
{
    public class Job
    {
        public string Name { get; set; }
        public Job DependsOn { get; set; }

        public Job(string jobName, Job jobDependsOn)
        {
            Name = jobName;
            DependsOn = jobDependsOn;
        }
    }
}
