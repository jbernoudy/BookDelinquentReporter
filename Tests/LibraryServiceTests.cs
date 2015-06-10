using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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


        Member mockMember1 = new Member()
        {
            Id = "1",
            FirstName = "Justin",
            LastName = "Horst"
        };

        Member mockMember2 = new Member()
        {
            Id = "2",
            FirstName = "Wes",
            LastName = "Peter"
        };

        Book mockBook1 = new Book()
        {
            Id = "1",
            Name = "Game of Thrones"
        };

        Book mockBook2 = new Book()
        {
            Id = "1",
            Name = "Star Wars"
        };

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

            mockDataLoadingService.Setup(m => m.GetCheckoutsAsync()).ReturnsAsync(new List<Checkout>() {mockCheckout1});
            mockDataLoadingService.Setup(m => m.GetMembersAsync()).ReturnsAsync(new List<Member>() {mockMember1});
            mockDataLoadingService.Setup(m => m.GetBooksAsync()).ReturnsAsync(new List<Book>() {mockBook1});

            LibraryService service = new LibraryService();
            var members = await service.GetDelinquentMembers();

            Assert.AreEqual(1, members.Count);
        }

        [TestMethod]
        public async Task MemberOverdueOnTwoBooksOnlyReturnedOnce()
        {
            Checkout mockCheckout1 = new Checkout()
            {
                UserId = "1",
                BookId = "1",
                CheckInDate = DateTime.Today.AddDays(-1)
            };

            Checkout mockCheckout2 = new Checkout()
            {
                UserId = "1",
                BookId = "2",
                CheckInDate = DateTime.Today.AddDays(-1)
            };

            Checkout mockCheckout3 = new Checkout()
            {
                UserId = "1",
                BookId = "3",
                CheckInDate = DateTime.Today.AddDays(1)
            };

            mockDataLoadingService.Setup(m => m.GetCheckoutsAsync()).ReturnsAsync(new List<Checkout>() { mockCheckout1, mockCheckout2, mockCheckout3 });
            mockDataLoadingService.Setup(m => m.GetMembersAsync()).ReturnsAsync(new List<Member>() { mockMember1 , mockMember2});
            mockDataLoadingService.Setup(m => m.GetBooksAsync()).ReturnsAsync(new List<Book>() { mockBook1, mockBook2 });

            LibraryService service = new LibraryService();
            var members = await service.GetDelinquentMembers();

            Assert.AreEqual(1, members.Count);
        }

        [TestMethod]
        public async Task NoDelinquentMembersReturnEmptyList()
        {
            Checkout mockCheckout1 = new Checkout()
            {
                UserId = "1",
                BookId = "1",
                CheckInDate = DateTime.Today.AddDays(1)
            };

            Checkout mockCheckout2 = new Checkout()
            {
                UserId = "1",
                BookId = "2",
                CheckInDate = DateTime.Today.AddDays(21)
            };

            Checkout mockCheckout3 = new Checkout()
            {
                UserId = "2",
                BookId = "3",
                CheckInDate = DateTime.Today.AddDays(1)
            };

            mockDataLoadingService.Setup(m => m.GetCheckoutsAsync()).ReturnsAsync(new List<Checkout>() { mockCheckout1, mockCheckout2, mockCheckout3 });
            mockDataLoadingService.Setup(m => m.GetMembersAsync()).ReturnsAsync(new List<Member>() { mockMember1, mockMember2 });
            mockDataLoadingService.Setup(m => m.GetBooksAsync()).ReturnsAsync(new List<Book>() { mockBook1, mockBook2 });

            LibraryService service = new LibraryService();
            var members = await service.GetDelinquentMembers();

            Assert.AreEqual(0, members.Count);
        }

        [TestMethod]
        public async Task GetAmountOwedForMemberWithOneLateBook()
        {
            Checkout mockCheckout1 = new Checkout()
            {
                UserId = "1",
                BookId = "1",
                CheckInDate = DateTime.Today.AddDays(-1)
            };

            Checkout mockCheckout2 = new Checkout()
            {
                UserId = "1",
                BookId = "2",
                CheckInDate = DateTime.Today.AddDays(21)
            };

            Checkout mockCheckout3 = new Checkout()
            {
                UserId = "2",
                BookId = "3",
                CheckInDate = DateTime.Today.AddDays(1)
            };

            mockDataLoadingService.Setup(m => m.GetCheckoutsAsync()).ReturnsAsync(new List<Checkout>() { mockCheckout1, mockCheckout2, mockCheckout3 });
            mockDataLoadingService.Setup(m => m.GetMembersAsync()).ReturnsAsync(new List<Member>() { mockMember1, mockMember2 });
            mockDataLoadingService.Setup(m => m.GetBooksAsync()).ReturnsAsync(new List<Book>() { mockBook1, mockBook2 });

            LibraryService ls = new LibraryService();

            Assert.AreEqual(0.3, ls.GetAmountOwed(mockMember1));

        }

        [TestMethod]
        public async Task GetAmountOwedForMemberWithNoLateBooks()
        {
            Checkout mockCheckout1 = new Checkout()
            {
                UserId = "1",
                BookId = "1",
                CheckInDate = DateTime.Today.AddDays(1)
            };

            Checkout mockCheckout2 = new Checkout()
            {
                UserId = "1",
                BookId = "2",
                CheckInDate = DateTime.Today.AddDays(21)
            };

            Checkout mockCheckout3 = new Checkout()
            {
                UserId = "2",
                BookId = "3",
                CheckInDate = DateTime.Today.AddDays(1)
            };

            mockDataLoadingService.Setup(m => m.GetCheckoutsAsync()).ReturnsAsync(new List<Checkout>() { mockCheckout1, mockCheckout2, mockCheckout3 });
            mockDataLoadingService.Setup(m => m.GetMembersAsync()).ReturnsAsync(new List<Member>() { mockMember1, mockMember2 });
            mockDataLoadingService.Setup(m => m.GetBooksAsync()).ReturnsAsync(new List<Book>() { mockBook1, mockBook2 });

            LibraryService ls = new LibraryService();

            Assert.AreEqual(0, ls.GetAmountOwed(mockMember1));
        }

        [TestMethod]
        public async Task GetAmountOwedForMemberWithTwoLateBooks()
        {
            Checkout mockCheckout1 = new Checkout()
            {
                UserId = "1",
                BookId = "1",
                CheckInDate = DateTime.Today.AddDays(-1)
            };

            Checkout mockCheckout2 = new Checkout()
            {
                UserId = "1",
                BookId = "2",
                CheckInDate = DateTime.Today.AddDays(-1)
            };

            Checkout mockCheckout3 = new Checkout()
            {
                UserId = "2",
                BookId = "3",
                CheckInDate = DateTime.Today.AddDays(1)
            };

            mockDataLoadingService.Setup(m => m.GetCheckoutsAsync()).ReturnsAsync(new List<Checkout>() { mockCheckout1, mockCheckout2, mockCheckout3 });
            mockDataLoadingService.Setup(m => m.GetMembersAsync()).ReturnsAsync(new List<Member>() { mockMember1, mockMember2 });
            mockDataLoadingService.Setup(m => m.GetBooksAsync()).ReturnsAsync(new List<Book>() { mockBook1, mockBook2 });

            LibraryService ls = new LibraryService();

            Assert.AreEqual(0.6, ls.GetAmountOwed(mockMember1));
        }

    }
}
