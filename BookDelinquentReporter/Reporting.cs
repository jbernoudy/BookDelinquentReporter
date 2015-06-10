using BookDelinquentReporter.Models;
using BookDelinquentReporter.Services;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDelinquentReporter
{
    public class Reporting
    {
        private List<DelinquencyReport> _delinquencyReports;
        private ILibraryService _libraryService
        {
            get { return SimpleIoc.Default.GetInstance<ILibraryService>(); }
        }

        public async Task<int> GetNumberOfDeliquentMembers()
        {
            var delinquentMembers = await _libraryService.GetDelinquentMembers();
            return delinquentMembers.Count;
        }

        public async Task<List<DelinquencyReport>> GetDeliquentMemberReports()
        {
            var delinquentMembers = await _libraryService.GetDelinquentMembers();
            _delinquencyReports = new List<DelinquencyReport>();
            foreach (var member in delinquentMembers)
            {
                var memberReport = new DelinquencyReport();
                memberReport.Member = member;
                memberReport.AmountOwed = _libraryService.GetAmountOwed(member);
                _delinquencyReports.Add(memberReport);
            }
            return _delinquencyReports;
        }
    }
}
