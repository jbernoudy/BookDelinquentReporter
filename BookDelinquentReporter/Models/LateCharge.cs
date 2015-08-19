using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDelinquentReporter.Models
{
    public class LateCharge
    {
        private const double BaseCharge = .35;
        public LateCharge(Book book, double fee, DateTime duedate)
        {
            Book = book;
            Fee = fee;
            DueDate = duedate;
        }

        public Book Book { get; set; }

        public double Fee { get; set; }

        public DateTime DueDate { get; set; }

    }
}
