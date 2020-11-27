using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OnTheBeachExercise
{
    public class JobSorter
    {
        private List<Job> theJobs = new List<Job>();

        public string SortJobs(string jobList)
        {
            // initialise jobs given the list passed by the client
            PrepareJobs(jobList);

            // Sort the jobs according to their dependency
            string jobSequence = ArrangeJobs();

            // return the ordered job sequence
            return jobSequence;
        }


        private void PrepareJobs(string jobList)
        {
            if (!string.IsNullOrEmpty(jobList))
            {
                // split the job list into each relationship
                string[] jobRelations = jobList.Split(',');
                foreach (string jobRelation in jobRelations)
                {
                    // Split the relationship 
                    string[] stringSeparators = new string[] { "=>" };
                    // Split a string delimited by another string and return all elements.
                    string[] jobDetails = jobRelation.Split(stringSeparators, StringSplitOptions.None);

                    PrepareJob(jobDetails[0].Trim(), jobDetails[1].Trim());
                }
            }
        }

        private void PrepareJob(string dependentName, string dependencyName)
        {
            // a => b  ----  a is dependent on b.  b is the dependency


            #region " Notes "
                // This was added to test SelfDependency
            #endregion
            if (dependentName == dependencyName)
            {
                throw new Exception("Jobs can't depend on themselves.");
            }

            Job dependency = CreateJob(dependencyName);
            Job dependent = CreateJob(dependentName);

            // Make the dependent point to the dependency
            dependent.DependsOn = dependency;

            theJobs.Add(dependent);
        }

        private Job CreateJob(string jobName)
        {
            if (!string.IsNullOrEmpty(jobName))
            {
                // find if job already exists
                var theJob = theJobs.Where(x => x.Name == jobName).FirstOrDefault();

                // if job does not exist, create one
                if (theJob is null)
                {
                    theJob = new Job(jobName, null);
                }

                return theJob;
            }
            return null;
        }


        private string ArrangeJobs()
        {
            #region " Notes "
            // 1st Attempt: list all jobs in theJobs with no sorting.  This satisfies  tests 1, 2 and 3
            // string orderedList = string.Join(", ", theJobs.Select(x => x.Name));
            #endregion " End Notes "

            // a => b  ----  a is dependent on b.  b is the dependency

            // First add all non-dependent jobs to the list
            var arrangedJobs = theJobs.Where(x => x.DependsOn == null).OrderBy(y=>y.Name).ToList();

            foreach(Job dependentJob in theJobs.Where(x=>x.DependsOn != null))
            {
                // Find the job index the dependent job depends on
                int dependencyPosition = arrangedJobs.FindIndex(x=>x.Name == dependentJob.DependsOn.Name);

                // Find the dependent job's position
                int dependentPosition = arrangedJobs.FindIndex(x => x.Name == dependentJob.Name);

                // Ensure that there is no circular dependencies (this happens when both dependent and dependency exist in arrangedJobs)
                if (dependencyPosition > -1 || dependentPosition > -1)
                {
                    throw new Exception("Jobs can't have circular dependencies.");
                }

                // if the dependency job is not in arranged jobs, add it to the end of arrangedJobs together with the dependent job
                if (dependencyPosition == -1)
                {
                    arrangedJobs.Add(dependentJob.DependsOn);
                    arrangedJobs.Add(dependentJob);
                }
                else
                {
                    // ensure that the dependent job does not exist on the arranged jobs yet
                    if (dependentPosition == -1)
                    {
                        // insert the dependent job immediately after the dependency job
                        arrangedJobs.Insert(dependencyPosition + 1, dependentJob);
                    }
                }
            }

            string orderedList =  string.Join(", ", arrangedJobs.Select(x => x.Name));               
            return orderedList;
        }
    }
}
