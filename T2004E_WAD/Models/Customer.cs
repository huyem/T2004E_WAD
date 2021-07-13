using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T2004E_WAD.Models
{
    public class Customer
    {

        public Customer(string name, string telephone, string address)
        {
            Name = name;
            Telephone = telephone;
            Address = address;
        }
        public String Name { get; set; }
        public String Telephone { get; set; }
        public String Address { get; set; }
    }
}