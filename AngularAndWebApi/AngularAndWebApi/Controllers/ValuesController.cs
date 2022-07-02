using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AngularAndWebApi.Models;
using CaseDiary.Service;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AngularAndWebApi.Controllers
{
   
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        ICaseDiaryRepository repository;
        public BL ObjBl { get; set; }
        public ValuesController(ICaseDiaryRepository repo)
        {
            repository = repo;
            ObjBl = new BL(repository);
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            StudentResult sr = new StudentResult();
            StudentSearch ss = new StudentSearch();
            Student s = new Student();
            List<Student> studentlist = new List<Student>();
            ss.StuCourse = 0;
            ss.StudentId = 0;
            ss.StuFirstName = "";
            ss.StuPhoneNo = "";
            //ss.StuRegistrationDate = null;
            studentlist = ObjBl.getmainitemdetails(ss, s, "niiast", "sp_GetAllOrSingleStudent1");
            return studentlist;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
