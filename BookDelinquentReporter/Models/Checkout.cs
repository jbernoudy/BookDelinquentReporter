using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDelinquentReporter.Models
{
    public class Checkout
    {
        public string UserId { get; set; }
        public string BookId { get; set; }
        public DateTime CheckInDate { get; set; }
    }
}
