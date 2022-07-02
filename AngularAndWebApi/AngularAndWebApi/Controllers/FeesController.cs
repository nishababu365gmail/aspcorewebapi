using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularAndWebApi.Models;
using CaseDiary.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularAndWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeesController : ControllerBase
    {
        ICaseDiaryRepository repository;
        public FeesController(ICaseDiaryRepository repo)
        {
        repository=repo;
            ObjBl = new BL(repository);
        }
        public BL ObjBl { get; private set; }
        public FeesPaymentMasterSearch ObjSearch { get; private set; }
        public FeesPayment ObjFeesPayment { get; private set; }
        public List<FeesPayment> ListFeesPayment { get; set; }
        // GET: api/Fees
        [HttpGet]
        public FeesPayment Get(int ? CourseId,int ? StudentId)
        {
            // return new string[] { "value1", "value2" };
            ObjSearch = new FeesPaymentMasterSearch();
            ObjFeesPayment = new FeesPayment();
            ListFeesPayment = new List<FeesPayment>();
            ObjSearch.CourseId = CourseId;
            ObjSearch.StudentId = StudentId;            
            ListFeesPayment = ObjBl.getmainitemdetails(ObjSearch, ObjFeesPayment, "niiast", "sp_GetFeesStudentCourseWise", 0);
            if (ListFeesPayment.Count > 0)
            {
                ObjFeesPayment = ListFeesPayment[0];
            }
            return ObjFeesPayment;
        }

        // GET: api/Fees/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Fees
        [HttpPost]
        public void Post([FromBody] FeesPayment fp)
        {
            ObjBl.InsertItems(fp, "niiast", "sp_save_masterfeespayment");
        }

        // PUT: api/Fees/5
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
