using System;
namespace ParkingLot.Models
{
    public class Ticket
    {
        public string Id { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public int SlotNumber { get; set; }
        public string VehicleNumber{ get; set; }
        public Ticket(int slotNumber,string vehicleNumber)
        {
            SlotNumber = slotNumber;
            VehicleNumber = vehicleNumber;
            
        }
        public Ticket()
        {
        }

    }
}
