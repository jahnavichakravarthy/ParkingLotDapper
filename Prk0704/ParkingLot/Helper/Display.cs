using System;
namespace ParkingLot.Helper
{
    public class Display
    {
        public void Print(string data)
        {
            Console.WriteLine($"{data}"); ;
        }

        public string Scan()
        {
            string data;
            data = Console.ReadLine();
            return data;
        }

    }
}
