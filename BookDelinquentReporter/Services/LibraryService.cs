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

        public Task<List<Member>> GetDelinquentMembers()
        {
            throw new NotImplementedException();
        }

        public float GetAmountOwed(Member m)
        {
            throw new NotImplementedException();
        }
    }
}
