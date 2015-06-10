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
        public async Task GetAllMembersNone()
        {
            mockDataLoadingService.Setup(m => m.GetMembersAsync()).ReturnsAsync(new List<Member>() {});

            LibraryService service = new LibraryService();
            var members = await service.GetAllMembers();

            Assert.AreEqual(0, members.Count);
        }

        [TestMethod]
        public async Task GetAllMembersOne()
        {
            mockDataLoadingService.Setup(m => m.GetMembersAsync()).ReturnsAsync(new List<Member>() { new Member()});

            LibraryService service = new LibraryService();
            var members = await service.GetAllMembers();

            Assert.AreEqual(1, members.Count);
        }

        [TestMethod]
        public async Task GetDelinquintMembersOne()
        {
            Checkout mockCheckout1 = new Checkout()
            {
                UserId = "1",
                BookId = "1",
                CheckInDate = DateTime.Today.AddDays(-1)
            };

            Member mockMember1 = new Member()
            {
                Id = "1",
                FirstName = "Justin",
                LastName = "Horst"
            };

            Book mockBook1 = new Book()
            {
                Id = "1",
                Name = "Game of Thrones"
            };

            mockDataLoadingService.Setup(m => m.GetCheckoutsAsync()).ReturnsAsync(new List<Checkout>() {mockCheckout1});
            mockDataLoadingService.Setup(m => m.GetMembersAsync()).ReturnsAsync(new List<Member>() {mockMember1});
            mockDataLoadingService.Setup(m => m.GetBooksAsync()).ReturnsAsync(new List<Book>() {mockBook1});

            LibraryService service = new LibraryService();
            var members = await service.GetDelinquentMembers();

            Assert.AreEqual(1, members.Count);
        }

    }
}
