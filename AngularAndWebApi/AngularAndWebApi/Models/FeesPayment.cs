using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularAndWebApi.Models
{
    public class FeesPayment
    {
        public int FeesPaymentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public Decimal Fees { get; set; }
        public Decimal FeesPaid { get; set; }
        public Decimal FeesDueAmount { get; set; }
        public DateTime FeesPaidDate { get; set; }
    }
}
