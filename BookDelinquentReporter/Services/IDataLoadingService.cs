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
        Task<Member> GetMembersAsync();

        Task<Book> GetBooksAsync();

        Task<Checkout> GetCheckoutsAsync();
    }
}
