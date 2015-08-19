using BookDelinquentReporter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDelinquentReporter
{
    public class DelinquencyReport
    {
        public Member Member { get; set; }

        public double AmountOwed { get; set; }

        public List<Book> OverdueBooks { get; set; }
    }
}
