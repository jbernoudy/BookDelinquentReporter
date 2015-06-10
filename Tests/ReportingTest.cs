using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookDelinquentReporter.Services;
using BookDelinquentReporter.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using GalaSoft.MvvmLight.Ioc;

namespace BookDelinquentReporter.Tests
{
    [TestClass]
    public class ReportingTest
    {
        private List<Member> _expectedDelinquentMembers;

        private Mock<ILibraryService> mockLibraryService;

        [TestInitialize]
        public void TestInit()
        {
            mockLibraryService = new Mock<ILibraryService>();

            SimpleIoc.Default.Unregister<ILibraryService>();
            SimpleIoc.Default.Register<ILibraryService>(() => mockLibraryService.Object);

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
        public void TestReportingWith0DeliquentMembers()
        {
            mockLibraryService.Setup(m => m.GetDelinquentMembers()).ReturnsAsync(new List<Member>() { });
            Reporting reporting = new Reporting();
            Assert.AreEqual(0, reporting.GetNumberOfDeliquentMembers());
        }

        [TestMethod]
        public void TestReportingWithKnownExpectedDeliquentMembers()
        {
            mockLibraryService.Setup(m => m.GetDelinquentMembers()).ReturnsAsync(_expectedDelinquentMembers);

            Reporting reporting = new Reporting();
            Assert.AreEqual(_expectedDelinquentMembers.Count, reporting.GetNumberOfDeliquentMembers());
        }
    }
}
