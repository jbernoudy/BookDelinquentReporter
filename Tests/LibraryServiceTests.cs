using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDelinquentReporter.Models;
using BookDelinquentReporter.Services;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests
{
    [TestClass]
    public class LibraryServiceTests
    {
        private Mock<IDataLoadingService> mockDataLoadingService;

        [TestInitialize]
        public void TestInit()
        {
            mockDataLoadingService = new Mock<IDataLoadingService>();

            SimpleIoc.Default.Unregister<IDataLoadingService>();
            SimpleIoc.Default.Register<IDataLoadingService>(() => mockDataLoadingService.Object);
        }

        [TestMethod]
        public void GetDelinquintMembersNone()
        {
            mockDataLoadingService.Setup(m => m.GetMembersAsync()).ReturnsAsync(new List<Member>() {});

            LibraryService service = new LibraryService();
            var members = service.GetAllMembers();

            Assert.AreEqual(0, members.Count);
        }

        [TestMethod]
        public void GetDelinquintMembersOne()
        {
            mockDataLoadingService.Setup(m => m.GetMembersAsync()).ReturnsAsync(new List<Member>() { new Member()});

            LibraryService service = new LibraryService();
            var members = service.GetAllMembers();

            Assert.AreEqual(1, members.Count);
        }



    }
}
