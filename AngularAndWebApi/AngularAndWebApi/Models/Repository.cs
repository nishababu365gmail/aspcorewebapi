using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AngularAndWebApi.Models
{
    public class Repository:IRepository
    {
        private Dictionary<int, Reservation> items;

        public Repository()
        {
            items = new Dictionary<int, Reservation>();
            new List<Reservation> {
                new Reservation {Id=1, Name = "Ankit", StartLocation = "New York", EndLocation="Beijing" },
                new Reservation {Id=2, Name = "Bobby", StartLocation = "New Jersey", EndLocation="Boston" },
                new Reservation {Id=3, Name = "Jacky", StartLocation = "London", EndLocation="Paris" }
                }.ForEach(r => AddReservation(r));
        }

        public Reservation this[int id] => items.ContainsKey(id) ? items[id] : null;

        public IEnumerable<Reservation> Reservations => items.Values;

        //public DataTable IRepository.Reservations1 => throw new NotImplementedException();

        public DataTable ReservationsTable()
        {
            DataTable dt = new DataTable();
            
            System.Data.DataTable custTable = new DataTable("Customers");
            DataColumn dtColumn;

            // Create id column
            dtColumn = new DataColumn();
            
            dtColumn.DataType = System.Type.GetType("System.String");
            dtColumn.ColumnName = "Name";
            dtColumn.Caption = "Name";
            dtColumn.ReadOnly = true;
            dtColumn.Unique = true;

            // Add id column to the DataColumnCollection.
            custTable.Columns.Add(dtColumn);
            dtColumn = new DataColumn();

            dtColumn.DataType = System.Type.GetType("System.String");
            dtColumn.ColumnName = "StartLocation";
            dtColumn.Caption = "StartLocation";
            dtColumn.ReadOnly = true;
            dtColumn.Unique = true;

            // Add id column to the DataColumnCollection.
            custTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();

            dtColumn.DataType = System.Type.GetType("System.String");
            dtColumn.ColumnName = "EndLocation";
            dtColumn.Caption = "EndLocation";
            dtColumn.ReadOnly = true;
            dtColumn.Unique = true;

            // Add id column to the DataColumnCollection.
            custTable.Columns.Add(dtColumn);
            DataRow row = custTable.NewRow();
            row["Name"] = "Ankit";
            row["StartLocation"] = "pos1";
            row["EndLocation"] = "posend";
            custTable.Rows.Add(row);
            return custTable;
        }


        public Reservation AddReservation(Reservation reservation)
        {
            if (reservation.Id == 0)
            {
                int key = items.Count;
                while (items.ContainsKey(key)) { key++; };
                reservation.Id = key;
            }
            items[reservation.Id] = reservation;
            return reservation;
        }

        public void DeleteReservation(int id) => items.Remove(id);

        public Reservation UpdateReservation(Reservation reservation) => AddReservation(reservation);
    }
}
