using System;
using ParkingLot.DBMethods;
using System.Collections.Generic;
using ParkingLot.Models;
using System.Linq;

namespace ParkingLot.Services
{
    interface IParkingService
    {
        List<Slot> GetAvailableSlots(VehicleModel type);
        void InitializeParkingLot(int twoWheeler, int fourWheeler, int heavyVehicles);
        void Park(int slotId, string vehicleNumber);
        void UnPark(int number);
        //List<Slot> GetSlots();
        Slot GetSlot(int Number);
       bool CheckSimialarVehicleNo(string newVehicleNo);
    }
    public class ParkingSevice : IParkingService
    {
        TicketService TicketService = new TicketService();
        public SlotsDb slotsDb = new SlotsDb();
        DBTickets DBTickets = new DBTickets();
        public List<Slot> Slots = new List<Slot>();
        
        public List<Slot> GetAvailableSlots(VehicleModel type)
        {
            Slots =slotsDb.GetSlots();
                 //this is to get the slots which are available for parking
                 var slots = from slot in Slots
                             where slot.Type == type && slot.Availability ==Status.AVAILABLE
                             select slot;
                 List<Slot> AvailableSlots = slots.ToList();
                 return (AvailableSlots.Count == 0) ? null : AvailableSlots;
            
        }
        public void InitializeParkingLot(int twoWheeler, int fourWheeler, int heavyVehicles)
        {
            //Intialize slots for various types of vehicles
            CreateSlot(twoWheeler, VehicleModel.TwoWheeler);
            CreateSlot(fourWheeler, VehicleModel.FourWheeler);
            CreateSlot(heavyVehicles, VehicleModel.HeavyVehicle);
        }
        public void CreateSlot(int NumberofVehicles, VehicleModel vehicleType)
        { 
            //create slots for each type of  vehicle
            for (int index = 1; index <=NumberofVehicles; index++)
            { 
                
                Slot slot = new Slot(vehicleType);
                slotsDb.Insert(slot);
               // Slots.Add(slot);
            }
        }
        public void Park(int slotId, string vehicleNumber)
        {
            //park a vehicle ,generate a tickeT,change status to "OCCUPIED"

            Slot SelectedSlot = Slots.Find(slot => slot.Id == slotId);
            SelectedSlot.Availability = Status.OCCUPIED;
            SelectedSlot.VehicleNumber= vehicleNumber;
            slotsDb.Update(SelectedSlot);
            
        }
        public void UnPark(int number)
        {
            Slots = slotsDb.GetSlots();
            //unpark a vehicle ,change the status to available
            Slot ThisSlot = Slots.Find(slot => slot.Id == number);
            ThisSlot.VehicleNumber = null;
            ThisSlot.Availability = Status.AVAILABLE;
            Ticket ticket = TicketService.GetTicket(number);
            ticket.OutTime = DateTime.Now;
            DBTickets.Update(ticket);
            slotsDb.Update(ThisSlot);
            
            

        }
        //public List<Slot> GetSlots()
        //{
        //    return Slots;
        //}
        public Slot GetSlot(int Number)
        {
            Slots = slotsDb.GetSlots();
            Slot SelectedSlot = Slots.Find(slot => slot.Id == Number);
            return SelectedSlot;
        }
        public bool CheckSimialarVehicleNo(string newVehicleNo)
        {
            Slots = slotsDb.GetSlots();
            try
            {
                //Slot slot = Slots.Find(item => item.ParkedVehicle.VehicleNumber == newVehicleNo);
                Slot slot = Slots.Find(item => item.VehicleNumber == newVehicleNo);
                return true;
            }
            catch (NullReferenceException)
            {
                return false;
            }


        }

    }
}