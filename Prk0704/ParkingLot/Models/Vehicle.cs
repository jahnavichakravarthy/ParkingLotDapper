
namespace ParkingLot.Models
{
    public class Vehicle
    {
        public VehicleModel Type { get; set; }
        public string VehicleNumber { get; set; }
    }
    public class TwoWheeler : Vehicle
    {
        public TwoWheeler(string vehicleNumber)
        {
            this.Type = VehicleModel.TwoWheeler;
            this.VehicleNumber = vehicleNumber;
        }
      
    }
    public class FourWheeler : Vehicle
    {
        public FourWheeler(string vehicleNumber)
        {
            this.Type = VehicleModel.FourWheeler;
            this.VehicleNumber = vehicleNumber;
        }
    }
    public class HeavyVehicle : Vehicle
    {
        public HeavyVehicle(string vehicleNumber)
        {
            this.Type = VehicleModel.HeavyVehicle;
            this.VehicleNumber = vehicleNumber;
        }
    }
}
