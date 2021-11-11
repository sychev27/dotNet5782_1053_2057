using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Customer
        {
            // int id;
            // string name;
            // string phone;
            // double longitude;
            // double latitude;

            public Customer(int _id, string _name, string _phone, double _longitude,
                double _latitude)
            {
                Id = _id;
                Name = _name;
                Phone = _phone;
                Longitude = _longitude;
                Latitude = _latitude;
            }

            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }

            //public void print()
            //{
            //    Console.WriteLine("Customer " + Id + " " + Name + "\n"+
            //        "Phone: " + Phone + "\n"+
            //        "(" + Longitude + "," + Longitude + ")" + "\n");
            //}

            //public static Customer Create() {
            //    Console.WriteLine("Please enter the customer's info:" + "\n" +
            //        "id, name, phone, longitude, latitude " + "/n");
            //    int id = 0;
            //    int.TryParse(Console.ReadLine(), out id);
            //    string name = Console.ReadLine();
            //    string phone = Console.ReadLine();
            //    double longitude = 0;
            //    double.TryParse(Console.ReadLine(), out longitude);
            //    double latitude = 0;
            //    double.TryParse(Console.ReadLine(), out latitude);
            //    Customer cust = new Customer(id, name, phone, longitude, latitude);
            //    return cust;
            //}

        }

    }

}
