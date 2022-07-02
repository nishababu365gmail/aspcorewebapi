using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularAndWebApi.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public int ReleasedYear { get; set; }
        public int DirectorId { get; set; }
        public string DirectorName { get; set; }
        public int CategoryId { get; set; }
        public string  CategoryName { get; set; }

    }
}
