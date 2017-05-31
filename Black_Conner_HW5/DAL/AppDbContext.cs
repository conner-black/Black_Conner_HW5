using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Black_Conner_HW5.Models;
using System.Data.Entity;

namespace Black_Conner_HW5.DAL
{
    public class AppDbContext : DbContext
    {
            //Constructor that invokes the base constructor
            public AppDbContext() : base("MyDBConnection") { }

            //Create the db set (class properties)
            public DbSet<Customer> Customers { get; set; }
            public DbSet<Frequency> Frequencies { get; set; }
    }
}