using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDelinquentReporter.Models
{
    public class LateCharge
    {
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
