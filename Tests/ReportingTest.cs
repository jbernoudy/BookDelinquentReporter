using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookDelinquentReporter.Services;
using BookDelinquentReporter.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookDelinquentReporter.Tests
{
    internal class LibraryServiceMock : ILibraryService
    {
        private List<Member> _delinquentMemebers;

        public LibraryServiceMock(List<Member> delinquentMemebers)
        {
            _delinquentMemebers = delinquentMemebers;
        }

        public Task<List<Member>> GetAllMembers()
        {
            throw new NotImplementedException();
        }

        public float GetAmountOwed(Member m)
        {
            throw new NotImplementedException();
        }

        public List<Member> GetDelinquentMembers()
        {
            return _delinquentMemebers;
        }

        Task<List<Member>> ILibraryService.GetDelinquentMembers()
        {
            throw new NotImplementedException();
        }
    }
    
    [TestClass]
    public class ReportingTest
    {
        private List<Member> _expectedDelinquentMembers;

        public ReportingTest()
        {
            _expectedDelinquentMembers = new List<Member>()
            {
                new Member() { FirstName = "John", LastName = "Doe", Id = "1" },
                new Member() { FirstName = "Jane", LastName = "Doe", Id = "2" },
                new Member() { FirstName = "Tom", LastName = "Doe", Id = "3" },
                new Member() { FirstName = "Dick", LastName = "Doe", Id = "4" },
                new Member() { FirstName = "Harry", LastName = "Doe", Id = "5" },
            };
        }

        [TestMethod]
        public void TestReportingWithNullService()
        {
            try
            {
                Reporting reporting = new Reporting(null);
                Assert.Fail("Expecting ArgumentNullException here.");
            }
            catch(Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentNullException));
            }
        }

        [TestMethod]
        public void TestReportingWith0DeliquentMembers()
        {
            Reporting reporting = new Reporting(new LibraryServiceMock(new List<Member>()));
            Assert.AreEqual(0, reporting.GetNumberOfDeliquentMembers());
        }

        [TestMethod]
        public void TestReportingWithKnownExpectedDeliquentMembers()
        {
            Reporting reporting = new Reporting(new LibraryServiceMock(_expectedDelinquentMembers));
            Assert.AreEqual(_expectedDelinquentMembers.Count, reporting.GetNumberOfDeliquentMembers());
        }

        [TestMethod]
        public void TestReportingGenerateReport()
        {
            Reporting reporting = new Reporting(new LibraryServiceMock(_expectedDelinquentMembers));
        }
    }
}
