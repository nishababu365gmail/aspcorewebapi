using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseDiary.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AngularAndWebApi.Models;
using System.Web.Http.Description;

namespace AngularAndWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        ICaseDiaryRepository repository;
        public BL ObjBl { get; set; }
        public TestController(ICaseDiaryRepository repo)
        {
            repository = repo;
            ObjBl = new BL(repository);
        }
        // GET: api/Test
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            // return new string[] { "value1", "value2" };
            StudentResult sr = new StudentResult();
            StudentSearch ss = new StudentSearch();
            Student s = new Student();
            List<Student> studentlist = new List<Student>();
            ss.StuCourse = null;
            ss.StudentId = null;
            ss.StuFirstName = null;
            ss.StuPhoneNo = null;
            ss.StuRegistrationDate = null;
            studentlist = ObjBl.getmainitemdetails(ss, s, "niiast", "sp_GetAllOrSingleStudent1");
            return studentlist;
        }
        [HttpGet("{search}")]
        public IEnumerable<Student> Search(string ? stuFirstName,string ?stuPhoneNo
            ,string? stuCourse,string ?StuRegistrationDate)
        {
            // return new string[] { "value1", "value2" };
           StudentResult sr = new StudentResult();
            StudentSearch ss = new StudentSearch();
            Student s = new Student();
            List<Student> studentlist = new List<Student>();
           
            ss.StuCourse = string.IsNullOrEmpty( stuCourse) ? ss.StuCourse=null : Convert.ToInt32(stuCourse);
            ss.StudentId = null;
            ss.StuFirstName = stuFirstName;
            ss.StuPhoneNo = stuPhoneNo;
          //  ss.StuRegistrationDate = StuRegistrationDate;
            if(StuRegistrationDate== "NaN-NaN-NaN")
            {
                ss.StuRegistrationDate = null; 
            }
            else
            {
                ss.StuRegistrationDate = StuRegistrationDate;
            }
            //ss.StuRegistrationDate = "2020-03-20";
            studentlist = ObjBl.getmainitemdetails(ss, s, "niiast", "sp_GetAllOrSingleStudent1");
            return studentlist;
        }
      //this is not used .If we want to send the search criteria in an object we can use the following way
        [HttpPost("{StudentSearch}", Name = "GetStudentSearch")]
        public IEnumerable<Student> Post([FromBody] StudentSearch ss)
        {
            // return new string[] { "value1", "value2" };
            StudentResult sr = new StudentResult();
            // StudentSearch ss = new StudentSearch();
            Student s = new Student();
            List<Student> studentlist = new List<Student>();
            ss.StuCourse = null;
            ss.StudentId = null;
            ss.StuFirstName = null;
            ss.StuPhoneNo = null;
             ss.StuRegistrationDate = null;
            studentlist = ObjBl.getmainitemdetails(ss, s, "niiast", "sp_GetAllOrSingleStudent1");
            return studentlist;
        }
        // GET: api/Test/5
        [HttpGet("{GetWithId}/{id}")]
        public Student SearchwithId(int id)
        {
            StudentResult sr = new StudentResult();
            StudentSearch ss = new StudentSearch();
            Student s = new Student();
            List<Student> studentlist = new List<Student>();
            ss.StuCourse = null;
            ss.StudentId = Convert.ToInt32(id);
            ss.StuFirstName = null;
            ss.StuPhoneNo = null;
            ss.StuRegistrationDate = null;
            studentlist = ObjBl.getmainitemdetails(ss, s, "niiast", "sp_GetAllOrSingleStudent1");
            if (studentlist.Count > 0)
            {
                s = studentlist[0];
            }

            return studentlist[0];

        }

        // POST: api/Test

        [HttpPost]
        public void Post([FromBody] Student s)
        {
            s.StuCourse = ObjBl.StudentCourse(s.newselectedItems);
            ObjBl.InsertItems(s, "niiast", "sp_savestudent");
        }

        // PUT: api/Test/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
