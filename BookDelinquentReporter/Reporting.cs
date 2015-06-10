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
        private ILibraryService _libraryService
        {
            get { return SimpleIoc.Default.GetInstance<ILibraryService>(); }
        }

        public async Task<int> GetNumberOfDeliquentMembers()
        {
            var delinquentMembers = await _libraryService.GetDelinquentMembers();
            return delinquentMembers.Count;
        }
    }
}
