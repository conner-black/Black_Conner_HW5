using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Black_Conner_HW5.Models
{
    public class Frequency
    {
        public Int32 FrequencyID { get; set; }

        public String Name { get; set; }

        //Navigational property for customers
        public List<Customer> Customers { get; set; }
    }
}