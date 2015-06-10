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

            var reporting = new Reporting();
            Assert.AreEqual(0, reporting.GetNumberOfDeliquentMembers().Result);
        }

        [TestMethod]
        public void TestReportingWith1DeliquentMembers()
        {
            mockLibraryService.Setup(m => m.GetDelinquentMembers()).ReturnsAsync(new List<Member>() { new Member() });

            var reporting = new Reporting();
            Assert.AreEqual(1, reporting.GetNumberOfDeliquentMembers().Result);
        }

        [TestMethod]
        public void TestReportingWithKnownExpectedDeliquentMembers()
        {
            mockLibraryService.Setup(m => m.GetDelinquentMembers()).ReturnsAsync(_expectedDelinquentMembers);

            var reporting = new Reporting();
            Assert.AreEqual(_expectedDelinquentMembers.Count, reporting.GetNumberOfDeliquentMembers().Result);
        }

        [TestMethod]
        public void TestReportingGetAmmountOwed1Delinquent()
        {
            var _oneDelinquentMember = new List<Member>()
            {
                new Member() { FirstName = "Larry", LastName = "Fitzgerald", Id = "1" }
            };
            mockLibraryService.Setup(m => m.GetDelinquentMembers()).ReturnsAsync(_oneDelinquentMember);
            mockLibraryService.Setup(m => m.GetAmountOwed(_oneDelinquentMember[0])).Returns(1.00f);

            var reporting = new Reporting();
            var _delinquencyReports = reporting.GetDeliquentMemberReports().Result;
            Assert.AreEqual(1.00f, _delinquencyReports[0].AmountOwed);
        }

        public void TestReportingGetAmmountOwed5Delinquent()
        {
            mockLibraryService.Setup(m => m.GetDelinquentMembers()).ReturnsAsync(_expectedDelinquentMembers);
            mockLibraryService.Setup(m => m.GetAmountOwed(_expectedDelinquentMembers[0])).Returns(1.00f);
            mockLibraryService.Setup(m => m.GetAmountOwed(_expectedDelinquentMembers[1])).Returns(2.00f);
            mockLibraryService.Setup(m => m.GetAmountOwed(_expectedDelinquentMembers[2])).Returns(3.00f);
            mockLibraryService.Setup(m => m.GetAmountOwed(_expectedDelinquentMembers[3])).Returns(4.00f);
            mockLibraryService.Setup(m => m.GetAmountOwed(_expectedDelinquentMembers[4])).Returns(5.00f);

            var reporting = new Reporting();
            var _delinquencyReports = reporting.GetDeliquentMemberReports().Result;
            Assert.AreEqual(1.00f, _delinquencyReports[0].AmountOwed);
            Assert.AreEqual(2.00f, _delinquencyReports[1].AmountOwed);
            Assert.AreEqual(3.00f, _delinquencyReports[2].AmountOwed);
            Assert.AreEqual(4.00f, _delinquencyReports[3].AmountOwed);
            Assert.AreEqual(5.00f, _delinquencyReports[4].AmountOwed);
        }
    }
}
