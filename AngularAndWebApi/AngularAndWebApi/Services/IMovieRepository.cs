using AngularAndWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularAndWebApi.Services
{
    public interface IMovieRepository
    {
        public void insertmovie(Movie model);
        public void updatemovie(Movie model);
        public Movie getmoviebyid(int id);
        public List<Movie> getallmovies();

        public void deletemovie(int id);
    }
}
