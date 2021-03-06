using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnTheBeachExercise;
using System;

namespace OnTheBeachTests
{
    [TestClass]
    public class TestJobs
    {
        [TestMethod]
        public void TestEmptyString()
        {
            JobSorter theSorter = new JobSorter();

            string jobList = "";
            string orderedList = theSorter.SortJobs(jobList);

            Assert.AreEqual(jobList, orderedList);
        }



        [TestMethod]
        public void TestSingleJob()
        {
            JobSorter theSorter = new JobSorter();

            string jobList = "a => ";
            string orderedList = theSorter.SortJobs(jobList);

            Assert.AreEqual(orderedList, "a");
        }


        [TestMethod]
        public void TestNonRelatedJobs()
        {
            JobSorter theSorter = new JobSorter();

            string jobList = "a => , b => , c => ";
            string orderedList = theSorter.SortJobs(jobList);

            Assert.AreEqual(orderedList, "a, b, c");
        }

        [TestMethod]
        public void TestSingleDependency()
        {
            JobSorter theSorter = new JobSorter();

            string jobList = "a => , b => c, c => ";
            string orderedList = theSorter.SortJobs(jobList);

            Assert.AreEqual(orderedList, "a, c, b");
        }


        [TestMethod]
        public void TestMultipleDependency()
        {
            JobSorter theSorter = new JobSorter();

            string jobList = "a => , b => c, c => f, d => a, e => b, f => ";
            string orderedList = theSorter.SortJobs(jobList);

            Assert.AreEqual(orderedList, "a, d, f, c, b, e");
        }


        [TestMethod]
        public void TestMultipleDependencyDifferentOrder()
        {
            // The ordered list should be the same as the one for TestMultipleDependency above
            JobSorter theSorter = new JobSorter();

            string jobList = "a =>, d => a, e => b, f => , c => f, b => c";
            string orderedList = theSorter.SortJobs(jobList);

            Assert.AreEqual(orderedList, "a, d, f, c, b, e");
        }

        [TestMethod]
        public void TestSelfDependency()
        {
            JobSorter theSorter = new JobSorter();

            string jobList = "a =>, b =>, c => c";
            string orderedList = "";

            var ex = Assert.ThrowsException<Exception>(() => orderedList = theSorter.SortJobs(jobList));

            Assert.AreEqual("Jobs can't depend on themselves.", ex.Message);
        }


        [TestMethod]
        public void TestCircularDependencies()
        {
            JobSorter theSorter = new JobSorter();

            string jobList = "a => , b => c, c => f, d => a, e => , f => b";
            string orderedList = "";

            var ex = Assert.ThrowsException<Exception>(() => orderedList = theSorter.SortJobs(jobList));

            Assert.AreEqual("Jobs can't have circular dependencies.", ex.Message);
        }

        [TestMethod]
        public void TestCircularDependenciesInDifferentOrder()
        {
            // The job list is the same as for TestCircularDependencies above but in a different order
            JobSorter theSorter = new JobSorter();

            string jobList = "d => a, a => , f => b, c => f, e => , b => c";
            string orderedList = "";

            var ex = Assert.ThrowsException<Exception>(() => orderedList = theSorter.SortJobs(jobList));

            Assert.AreEqual("Jobs can't have circular dependencies.", ex.Message);
        }
    }
}