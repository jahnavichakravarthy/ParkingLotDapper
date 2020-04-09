using ParkingLot.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections.Generic;

namespace ParkingLot.DBMethods
{
    public class SlotsDb
    {
        public void Insert(Slot slot)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ParkingLotDataBase"].ConnectionString))
            //ConfigurationManager.ConnectionStrings["ParkingLotDataBase"].ConnectionString))
            {
                string sqlQuery = "INSERT INTO SLOTS (Type, Availability) Values (@Type, @Availability);";
                int rowsAffected = db.Execute(sqlQuery, slot);
            }

        }

        public void ReInitialize()
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ParkingLotDataBase"].ConnectionString))
            {
                string sqlQuery = "DELETE FROM SLOTS";
                int rowsAffected = db.Execute(sqlQuery);
                //return rowsAffected;//returns number opf rows updated
            }

        }
        public List<Slot> GetSlots()
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ParkingLotDataBase"].ConnectionString))
            {
                return db.Query<Slot>("Select * From SLOTS").ToList();

            }

        }
        public int Update(Slot slot)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ParkingLotDataBase"].ConnectionString))
            {
                string sqlQuery = "UPDATE SLOTS SET Availability = @Availability, " +
                " VehicleNumber = @VehicleNumber " + " WHERE Id = @Id ";
                int rowsAffected = db.Execute(sqlQuery, slot);
                return rowsAffected;//returns number opf rows updated
            }
        }
    }
}
