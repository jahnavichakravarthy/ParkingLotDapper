using System;
using System.Collections.Generic;
using ParkingLot.Models;
using ParkingLot.Services;
using ParkingLot.Helper;
using ParkingLot.DBMethods;
using System.Linq;

namespace ParkingLot
{
    public class Program
    {
        public static void Main()
        {
            while (true)
            {
                Display Display = new Display();
                Functions functions = new Functions();
                SlotsDb slotsDb = new SlotsDb();
                DBTickets dBTickets = new DBTickets();
                List<Ticket> tickets = new List<Ticket>();
                IParkingService ParkingServices = new ParkingSevice();
                ITicketService ticketService = new TicketService();
                int option;
                string regionalCode;
                slotsDb.ReInitialize();
                dBTickets.ReInitialize();
                Display.Print("WELCOME TO ABCD PARKING LOT");
                Display.Print("******************************************");
                Display.Print("please enter number of slots for 2 wheeler");
                int twoWheeler = functions.Input();
                Display.Print("please enter number of slots for 4 wheeler");
                int fourWheeler = functions.Input();
                Display.Print("please enter number of slots for heavy vehicle");
                int heavyVehicle = functions.Input();
                Display.Print("do you want to enter the Regional Code\n\t1.yes\n\t2.no");
                option = functions.Input();
                switch (option)
                {
                    case 1:
                        Display.Print("please enter a Regional Code:");
                        regionalCode = Display.Scan();
                        functions.Regionalcode(regionalCode);
                        break;
                    default:
                        regionalCode = null;
                        break;
                }
                ParkingServices.InitializeParkingLot(twoWheeler, fourWheeler, heavyVehicle);
                bool Exit = false;
                do
                {
                    Display.Print("select a option\n\t1.park\n\t2.Un-park\n\t3.Display status of all slots\n\t4.Exit(Re-Initialize)");
                    option = functions.Input();
                    switch (option)
                    {
                        case 1:
                            Display.Print("please enter the valid details of the vehicle");
                            Vehicle vehicle = new Vehicle();
                            Display.Print("Choose the type of vehicle\n\t1.2 wheeler\n\t2.4 wheeler\n\t3.heavy vehicle\n\t");
                            int choice = functions.Input();
                            var Type = (VehicleModel)choice;
                            vehicle.Type=Type;
                            List<Slot> AvailableSlots = ParkingServices.GetAvailableSlots(Type);
                            if (AvailableSlots != null)
                            {
                                bool validSlot = false;//FOR EXITING THE LOOP
                                do
                                {
                                    foreach (Slot thisSlot in AvailableSlots)
                                    {
                                        Console.WriteLine($"slot-{ thisSlot.Id }");
                                    }
                                    Display.Print("Please choose a slot number");
                                    int slotNumber = functions.Input();
                                    Slot SelectedSlot = AvailableSlots.Find(slot => slot.Id == slotNumber);
                                    bool validity = false;
                                    if (SelectedSlot != null)
                                    {
                                        do
                                        {
                                            Display.Print("Please enter the vehicle number");
                                            string number = Display.Scan();
                                            var VehicleNumber = functions.vehicleNumber(number, regionalCode);
                                            switch (choice)
                                            {
                                                case 1:
                                                    vehicle = new TwoWheeler(VehicleNumber);
                                                    break;
                                                case 2:
                                                    vehicle = new FourWheeler(VehicleNumber);
                                                    break;
                                                case 3:
                                                    vehicle = new HeavyVehicle(VehicleNumber);
                                                    break;
                                                default:
                                                    break;
                                            }
                                            Validations validation = new Validations();
                                            //her vehicle numbr



                                            if (validation.IsValidVehicleNumber(number) == true)
                                            {
                                                //bool isSame = ParkingServices.CheckSimialarVehicleNo(vehicle.VehicleNumber);
                                                //if (isSame == false)
                                                //{
                                                validity = true;
                                                //}
                                                //else
                                                //{
                                                //    Display.Print("The Vehicle numberis already parked in another slot");
                                                //    Display.Print("******************************************");
                                                //}
                                                ////*********there is still a chance that two vehicles can be parked with same number
                                            }
                                            else
                                            {
                                                Display.Print("Please check the format of the vehicle number");
                                                Display.Print("******************************************");
                                                validity = false;
                                            }
                                        }
                                        while (validity == false);
                                        //SelectedSlot.ParkedVehicle = vehicle;
                                        SelectedSlot.VehicleNumber = vehicle.VehicleNumber;
                                        Ticket ticket = ticketService.GenerateTicket(slotNumber, vehicle.VehicleNumber);
                                        ParkingServices.Park(slotNumber, vehicle.VehicleNumber);
                                        //ticketService.UpdateTicketList(ticket);
                                        functions.Parked(SelectedSlot.Id);
                                        Display.Print("******************************************");
                                        Console.WriteLine($"Ticket Id:{ticket.Id}\nVehicle Number:{SelectedSlot.VehicleNumber}\nSlot Number:{SelectedSlot.Id}\nIn-Time:{ticket.InTime}");
                                        Display.Print("******************************************");
                                        validSlot = true;
                                    }
                                    else
                                    {
                                        functions.Invalid();
                                    }
                                }
                                while (validSlot == false);
                            }
                            else
                            {
                                functions.NoSlot();
                            }

                            break;
                        case 2:
                            while (true)
                            {

                                int ticketCount = ticketService.GetTicketCount();
                                if (ticketCount != 0)
                                {
                                    bool validSlot = false;
                                    do
                                    {
                                        Display.Print("Please enter the slot Id");
                                        int Id = functions.Input();
                                        Ticket SelectedTicket = ticketService.GetTicket(Id);
                                        if (SelectedTicket != null)
                                        {
                                            Slot slot = ParkingServices.GetSlot(Id);
                                            if (slot.Availability == Status.OCCUPIED)
                                            {
                                                //SelectedTicket.OutTime = DateTime.Now;
                                                ParkingServices.UnPark(Id);
                                                Display.Print("******************************************");
                                                Display.Print("The vehicle is unparked");
                                                Console.WriteLine($"Ticket Id:{SelectedTicket.Id}\nVehicle Number:{SelectedTicket.VehicleNumber}\nSlot Number:{SelectedTicket.SlotNumber}\nIn-Time:{SelectedTicket.InTime}\nOut-Time:{SelectedTicket.OutTime}");
                                                Display.Print("******************************************");
                                            }
                                            else
                                            {
                                                Display.Print("the vehicle is already unparked from the slot");
                                                Display.Print("******************************************");
                                            }
                                            validSlot = true;
                                        }
                                        else
                                        {
                                            Display.Print("enter a valid slot id");
                                        }
                                    }
                                    while (validSlot == false);
                                    break;
                                }
                                else
                                {
                                    Display.Print("no tickets are generated yet");
                                    break;
                                }

                            }
                                break;
                        case 3:
                            List<Slot> Slots = slotsDb.GetSlots();
                            foreach (Slot slot in Slots)
                            {
                                string vehicleNumber = ((slot.VehicleNumber == null) ? "-----" : slot.VehicleNumber);
                                Console.WriteLine($"slot:{slot.Id}  type:{slot.Type}  status:{Convert.ToString(slot.Availability)} vehiclePArked:{vehicleNumber}");
                            }
                            Display.Print("******************************************");
                            break;
                        case 4:
                            Exit = true;
                            slotsDb.ReInitialize();
                            dBTickets.ReInitialize();
                            break;
                        default:
                            Display.Print("please select a valid option");
                            Display.Print("******************************************");
                            break;
                    }

                }
                while (Exit == false);
            }
        }

    }
}