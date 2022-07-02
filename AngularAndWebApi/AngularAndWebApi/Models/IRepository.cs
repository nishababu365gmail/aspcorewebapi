using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AngularAndWebApi.Models
{
    public interface IRepository
    {
        IEnumerable<Reservation> Reservations { get; }
        DataTable ReservationsTable();
        Reservation this[int id] { get; }
        Reservation AddReservation(Reservation reservation);
        Reservation UpdateReservation(Reservation reservation);
        void DeleteReservation(int id);
    }
}
