using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AngularAndWebApi.Services;
using AngularAndWebApi.Models;
using System.Text.Json;

namespace AngularAndWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _repo;
        public MovieController(IMovieRepository repo)
        {
            _repo = repo;
        }
        // [Route("All")]
         //urlhttp://localhost:59075/api/movie
        public List<Movie> GetAllMovies()
        {
            return _repo.getallmovies();
        }
        [Route("insertvitems")]
        [HttpPost]
        public void insertVazhipadu( VazhipaduItem body )
        {
            string json = System.Text.Json.JsonSerializer.Serialize(body);
      VazhipaduItem myitem=      System.Text.Json.JsonSerializer.Deserialize<VazhipaduItem>(json);
            //VazhipaduItem test = n;
        }
        [Route("VItems")]
        public List<VazhipaduItem> GetAllVItems()
        {
            List<VazhipaduItem> lst = new List<VazhipaduItem>();
            VazhipaduItem obj = new VazhipaduItem();
            obj.clientname = "nisha";
            obj.vazhipaduname = "Ada";
            obj.starname = "Bharani";
            obj.vazhipaduqty = 12;
            obj.vazhipadurate = 90;
            obj.vazhipaduamount = 1080;
            lst.Add(obj);
            return lst;
        }
        //urlhttp://localhost:59075/api/movie/1
        [Route("{id}")]
        public Movie GetMovieById(int id)
        {
            return _repo.getmoviebyid(id);
        }

        [Route("hello/{id}")]
        public Movie NishaById(int id)
        {
            return _repo.getmoviebyid(id);
        }
    }
}
