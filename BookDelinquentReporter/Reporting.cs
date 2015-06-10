using BookDelinquentReporter.Models;
using BookDelinquentReporter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDelinquentReporter
{
    public class Reporting
    {
        private ILibraryService _libraryService;

        public Reporting(ILibraryService libraryService)
        {
            if(libraryService == null)
            {
                throw new ArgumentNullException("libraryService can not be null");
            }
            _libraryService = libraryService;
        }

        public async Task<int> GetNumberOfDeliquentMembers()
        {
            var delinquentMembers = await _libraryService.GetDelinquentMembers();
            return delinquentMembers.Count;
        }
    }
}
