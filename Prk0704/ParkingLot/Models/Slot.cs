
namespace ParkingLot.Models
{
    public class Slot
    {
        public int Id { get; set; }
        public VehicleModel Type { get; set; }
        public Status Availability { get; set; }
        //public Vehicle ParkedVehicle { get; set; }
        public string VehicleNumber{ get; set; }
        public Slot(VehicleModel type)
        {
            
            Type = type;
          Availability=(Status.AVAILABLE);
        }
        public Slot()
        {
            Availability = Status.AVAILABLE;
        }
    }
}
