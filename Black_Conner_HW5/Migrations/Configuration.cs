namespace Black_Conner_HW5.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Black_Conner_HW5.DAL.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Black_Conner_HW5.DAL.AppDbContext context)
        {
            //call method to add frequencies
            AddCustomers.AddFrequencies();

            //call method to add customers
            AddCustomers.SeedCustomers();
        }
    }
}
