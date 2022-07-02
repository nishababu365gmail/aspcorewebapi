using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularAndWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularAndWebApi.Controllers
{
    
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        // GET: api/BlogPost
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/BlogPost/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/BlogPost
        [HttpPost]
        public void Post([FromBody] BlogPost blogPost) 
        {
        }

        // PUT: api/BlogPost/5
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
