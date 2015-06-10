using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDelinquentReporter.Models;

namespace BookDelinquentReporter.Services
{
    public interface IDataLoadingService
    {
        Task<List<Member>> GetMembersAsync();

        Task<List<Book>> GetBooksAsync();

        Task<List<Checkout>> GetCheckoutsAsync();
    }
}
