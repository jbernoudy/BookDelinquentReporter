using BookDelinquentReporter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDelinquentReporter.Services
{
    public interface ILibraryService
    {
        Task<List<Member>> GetAllMembers();
        Task<List<Member>> GetDelinquentMembers();
        double GetAmountOwed(Member m);
        Task<List<LateCharge>> GetLateChargesForMember(Member m);
    }
}
