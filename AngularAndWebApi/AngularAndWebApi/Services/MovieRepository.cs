using AngularAndWebApi.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularAndWebApi.Services
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IConfiguration _config;

        public void deletemovie(int id)
        {
            throw new NotImplementedException();
        }
        private MySqlConnection GetSqlConnection()
        {
            string connectionstring = _config.GetConnectionString("ApiConnection");
            MySqlConnection con = new MySqlConnection(connectionstring);
            return con;
        }
        public MovieRepository(IConfiguration config)
        {
            _config = config;
        }
        public List<Movie> getallmovies()
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "sp_get_AllMovies";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            List<Movie> lst = new List<Movie>();
            Movie mv = null;
            using (var mycon = GetSqlConnection())
            {
                mycon.Open();
                cmd.Connection = mycon;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) { 
                    mv = new Movie();
                    mv.MovieId = Convert.ToInt32(reader["movieid"]);
                    mv.MovieName = reader["moviename"].ToString();
                    mv.DirectorName = reader["directorname"].ToString();
                    mv.CategoryName = reader["name"].ToString();
                    mv.ReleasedYear = Convert.ToInt32(reader["releasedyear"]);
                    lst.Add(mv);
                    }
                }
            }
            return lst;
        }

        public Movie getmoviebyid(int id)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "sp_get_MovieById";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            MySqlParameter[] pms = new MySqlParameter[4];
            pms[0] = new MySqlParameter("argmovieid", MySqlDbType.Int32);
            pms[0].Value = id;
            cmd.Parameters.Add(pms[0]);
            Movie mv = null;
            using (var mycon = GetSqlConnection())
            {
                mycon.Open();
                cmd.Connection = mycon;
               using(var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) { 
                    mv = new Movie();
                    mv.MovieId= Convert.ToInt32(reader["movieid"]);
                    mv.MovieName = reader["moviename"].ToString();
                    mv.DirectorName = reader["directorname"].ToString();
                    mv.CategoryName = reader["name"].ToString();
                    mv.ReleasedYear = Convert.ToInt32(reader["releasedyear"]);
                }
                }
            }
            return mv;
        }

        public void insertmovie(Movie model)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "sp_insert_movies";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            MySqlParameter[] pms = new MySqlParameter[4];
            pms[0] = new MySqlParameter("argmoviename",MySqlDbType.VarChar);
            pms[0].Value = model.MovieName;
            cmd.Parameters.Add(pms[0]);
            pms[0] = new MySqlParameter("argdirectorid", MySqlDbType.Int32);
            pms[0].Value = model.MovieId;
            cmd.Parameters.Add(pms[1]);
            pms[0] = new MySqlParameter("argreleasedyear", MySqlDbType.Int32);
            pms[0].Value = model.ReleasedYear;
            cmd.Parameters.Add(pms[2]);
            pms[0] = new MySqlParameter("argcategoryid", MySqlDbType.Int32);
            pms[0].Value = model.CategoryId;
            cmd.Parameters.Add(pms[3]);

            using (var mycon=GetSqlConnection())
            {
                mycon.Open();
                cmd.Connection = mycon;
                cmd.ExecuteNonQuery();
            }
        }

        public void updatemovie(Movie model)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "sp_update_movie";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            MySqlParameter[] pms = new MySqlParameter[5];
            pms[0] = new MySqlParameter("argmoviename", MySqlDbType.VarChar);
            pms[0].Value = model.MovieName;
            cmd.Parameters.Add(pms[0]);
            pms[1] = new MySqlParameter("argdirectorid", MySqlDbType.Int32);
            pms[1].Value = model.MovieId;
            cmd.Parameters.Add(pms[1]);
            pms[2] = new MySqlParameter("argreleasedyear", MySqlDbType.Int32);
            pms[2].Value = model.ReleasedYear;
            cmd.Parameters.Add(pms[2]);
            pms[3] = new MySqlParameter("argcategoryid", MySqlDbType.Int32);
            pms[3].Value = model.CategoryId;
            cmd.Parameters.Add(pms[3]);
            pms[4] = new MySqlParameter("argmovieid", MySqlDbType.Int32);
            pms[4].Value = model.CategoryId;
            cmd.Parameters.Add(pms[4]);
            using (var mycon = GetSqlConnection())
            {
                mycon.Open();
                cmd.Connection = mycon;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
