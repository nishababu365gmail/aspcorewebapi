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
    public class ExtraServiceController : ControllerBase
    {
        ICaseDiaryRepository repository;
        public BL ObjBl { get; set; }
        // GET: api/ExtraService
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        public ExtraServiceController(ICaseDiaryRepository repo)
        {
            repository = repo;
            
            ObjBl = new BL(repository);
        }
        // GET: api/ExtraService/5
        [HttpGet("{id}", Name = "GetDropVals")]
        public IEnumerable<SelectVals> Get(string  id)
        {
            Dictionary<int, string>dictvals= ObjBl.GetDropdownlistValsFromDatabase("", id, 0);
            List<SelectVals> ls = new List<SelectVals>();
            ls = ObjBl.DictToList(dictvals);
            return ls;
        }
        [HttpGet("{argRelationName}/{id}", Name = "GetDropSelVals")]
        public IEnumerable<SelectVals> Get(string argRelationName, string id)
        {
            Dictionary<int, string> dictvals = ObjBl.GetCardValsFromDatabase(argRelationName, Convert.ToInt32(id));
            List<SelectVals> ls = new List<SelectVals>();
            ls = ObjBl.DictToList(dictvals);
            return ls;
        }
        // POST: api/ExtraService
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ExtraService/5
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
