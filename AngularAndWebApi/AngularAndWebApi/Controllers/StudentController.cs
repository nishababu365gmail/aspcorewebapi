using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularAndWebApi.Models;
using CaseDiary.Service;
using Microsoft.AspNetCore.Mvc;

namespace AngularAndWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ICaseDiaryRepository _repo;

        public StudentController(ICaseDiaryRepository repo)
        {
            _repo = repo;
            
        }
        [Route("insertstudent")]
        [HttpPost]
        public int CreateStudent(Student1 mdl)
        { 
                         _repo.InsertItems<Student1>(ref mdl, "sakila", "sp_insert_student");
            return 1;
        }
    }
}
