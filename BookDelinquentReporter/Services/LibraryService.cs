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
        private IDataLoadingService _dataLoadingService => SimpleIoc.Default.GetInstance<IDataLoadingService>();

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

        public float GetAmountOwed(Member m)
        {
            throw new NotImplementedException();
        }
    }
}
