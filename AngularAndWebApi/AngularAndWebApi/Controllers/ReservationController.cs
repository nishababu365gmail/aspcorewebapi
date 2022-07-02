using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AngularAndWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace AngularAndWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private IRepository repository;
        public ReservationController(IRepository repo) => repository = repo;

        [HttpGet]
        public DataTable Get() => repository.ReservationsTable();
        //  public IEnumerable<Reservation> Get() => repository.Reservations;

        [HttpGet("{id}")]
        public ActionResult<Reservation> Get(int id)
        {
            if (id == 0)
                return BadRequest("Value must be passed in the request body.");
            return Ok(repository[id]);
        }

        [HttpPost]
        public Reservation Post([FromBody] Reservation res) =>
        repository.AddReservation(new Reservation
        {
            Name = res.Name,
            StartLocation = res.StartLocation,
            EndLocation = res.EndLocation
        });

        [HttpPut]
        public Reservation Put([FromBody] Reservation res) => repository.UpdateReservation(res);

        [HttpPatch("{id}")]
        public StatusCodeResult Patch(int id, [FromBody] JsonPatchDocument<Reservation> patch)
        {
            var res = (Reservation)((OkObjectResult)Get(id).Result).Value;
            if (res != null)
            {
                patch.ApplyTo(res);
                return Ok();
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public void Delete(int id) => repository.DeleteReservation(id);
    }
}
