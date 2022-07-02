using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularAndWebApi.Models
{
    public class FeesPaymentMasterSearch
    {
        public int? StudentId { get; set; }
        public int? CourseId { get; set; }
        public int? FeesPaymentMasterId { get; set; }
    }
}
