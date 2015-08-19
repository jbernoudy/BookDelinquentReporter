using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDelinquentReporter.Models;
using GalaSoft.MvvmLight.Ioc;

namespace BookDelinquentReporter.Services
{
    public class LibraryService : ILibraryService
    {
        private IDataLoadingService _dataLoadingService
        {
            get { return SimpleIoc.Default.GetInstance<IDataLoadingService>(); }
        }

        public async Task<List<Member>> GetAllMembers()
        {
            return await _dataLoadingService.GetMembersAsync();
        }

        public async Task<List<Member>> GetDelinquentMembers()
        {
            var members = await _dataLoadingService.GetMembersAsync();
            var books = await _dataLoadingService.GetBooksAsync();
            var checkouts = await _dataLoadingService.GetCheckoutsAsync();

            var overdueCheckouts = checkouts.Where(x => x.CheckInDate < DateTime.Now);
            
            List<Member> overdueMembers = new List<Member>();
            foreach (var member in overdueCheckouts.Select(overdue => members.First(x => x.Id == overdue.UserId)).Where(member => !overdueMembers.Contains(member)))
            {
                overdueMembers.Add(member);
            }

            return overdueMembers;
        }

        public double GetAmountOwed(Member m)
        {
            // todo: make method async
            var members =  _dataLoadingService.GetMembersAsync().Result;
            var books =  _dataLoadingService.GetBooksAsync().Result;
            var checkouts = _dataLoadingService.GetCheckoutsAsync().Result;

            var overdueCheckouts = checkouts.Where(x => x.CheckInDate < DateTime.Now && x.UserId == m.Id);

            double totalDue = 0;
            double feePerDay = 0.3;

            foreach (var checkOut in overdueCheckouts)
            {
                var daysOver = (DateTime.Today - checkOut.CheckInDate).Days;
                totalDue += daysOver*feePerDay;
            }

            return totalDue;
        }

        public async Task<List<LateCharge>> GetLateChargesForMember(Member m)
        {
            // todo: make method async
            var members = await _dataLoadingService.GetMembersAsync();
            var books = await _dataLoadingService.GetBooksAsync();
            var checkouts = await _dataLoadingService.GetCheckoutsAsync();

            var overdueCheckouts = checkouts.Where(x => x.CheckInDate < DateTime.Now && x.UserId == m.Id);

            double totalDue = 0;
            double feePerDay = 0.3;
            List<LateCharge> charges = new List<LateCharge>();

            foreach (var checkOut in overdueCheckouts)
            {
                var daysOver = (DateTime.Today - checkOut.CheckInDate).Days;
                var fee = daysOver * feePerDay;

                // todo: handle book does not exist
                charges.Add(new LateCharge(books.FirstOrDefault(x => x.Id.Equals(checkOut.BookId)), fee, checkOut.CheckInDate));
            }

            return charges;
        }
    }
}
