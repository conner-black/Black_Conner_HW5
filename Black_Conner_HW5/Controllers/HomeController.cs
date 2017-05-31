using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Black_Conner_HW5.DAL;
using Black_Conner_HW5.Models;
using Black_Conner_HW5.Migrations;

namespace Black_Conner_HW5.Controllers
{
    //Enums for the advanced search
    public enum Gender {All, Male, Female}
    public enum Order {Greater, Less} //Enum for greater than/less than radio buttons

    public class HomeController : Controller
    {
        //Create an instance of the context
        private AppDbContext db = new AppDbContext();
        
        // GET: Home
        public ActionResult Index(string SearchString)
        {
            //Add total customer count to viewbag
            ViewBag.TotalCustomerCount = db.Customers.ToList().Count();

            //Create a list of customers that will contain correct customers
            List<Customer> SelectedCustomers = new List<Customer>();

            if (SearchString == null)
            {
                //Making counts valid if search string is null
                ViewBag.SelectedCount = ViewBag.TotalCustomerCount;
                return View(db.Customers.ToList());
            }
            else
            {
                //Select customers where FName/LName contains search string
                SelectedCustomers = db.Customers.Where(c => c.FirstName.Contains(SearchString) || c.LastName.Contains(SearchString)).ToList();
                
                //Count the selected records
                ViewBag.SelectedCount = SelectedCustomers.Count();

                //Order selected customers by last name, first name, and average sales
                var sortedCustomerList = SelectedCustomers.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ThenBy(c => c.AverageSale);

                //Return selected customers
                return View(sortedCustomerList);
            }
        }
        public ActionResult DetailedSearch()
        {
            ViewBag.AllFrequencies = GetAllFrequencies(); //Get all frequencies in the database for the dropdown menu option
            return View();
        }

        public ActionResult SearchResults(string NameString, int Frequency, Gender gender, string strSalesAmount, Order? Direction)
        {
            //Add total customer count to viewbag
            ViewBag.TotalCustomerCount = db.Customers.ToList().Count();

            decimal decSalesAmount; //Variable for later

            //Draw query to show all possible customers (subsequent steps will filter query accordingly)
            var query = from c in db.Customers
            select c;

            //Filter query to only show names containing user-inputed NameString
            if(NameString!=null && NameString != "")
            {
                query = query.Where(c => c.FirstName.Contains(NameString) || c.LastName.Contains(NameString));
            }

            //If frequency is not equal to SelectAll option, filter accordingly
            if(Frequency != 0)
            {
                query = query.Where(c => c.Frequency.FrequencyID == Frequency);
            }

            //Select gender
            if (gender == Gender.Male)
            {
                query = query.Where(c => c.Gender == "Male");
            }
            if(gender == Gender.Female)
            {
                query = query.Where(c => c.Gender == "Female");
            }

            //Convert string input (Average Sales Amount) to decimal for comparison purposes
            try
            {
                //Convert sales amount string to a decimal
                decSalesAmount = Convert.ToDecimal(strSalesAmount);

                //Determines whether to return sales greater or less than the given amount based upon user selection
                if (Direction == Order.Greater)
                {
                    query = query.Where(c => c.AverageSale >= decSalesAmount);
                }
                if (Direction == Order.Less)
                {
                    query = query.Where(c => c.AverageSale <= decSalesAmount);
                }
            }
            catch
            {
                //If sales amount string is null, do not exit
                if(strSalesAmount != null)
                {
                    ViewBag.Message = "Invalid Sales Amount";
                    return View("DetailedSearch");
                }
            }

            //Sort query results
            query = query.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ThenBy(c => c.AverageSale);

            //Convert to list
            List<Customer> SelectedCustomers = query.ToList();

            //Count the number of customers in the remaining query
            ViewBag.SelectedCount = SelectedCustomers.Count();

            //Return desired customers
            return View("Index", SelectedCustomers);
        }

        public SelectList GetAllFrequencies() //Method to get all frequencies in order to populate the drop down list
        {
            List<Frequency> Freq_List = db.Frequencies.ToList();

            //Add an option to select all frequencies
            Frequency SelectAll = new Frequency { FrequencyID = 0, Name = "All Frequencies" };
            Freq_List.Add(SelectAll);

            //convert list to select list
            SelectList AllFrequenciesList = new SelectList(Freq_List.OrderBy(c => c.FrequencyID),"FrequencyID","Name");

            //return the select list
            return AllFrequenciesList;
        }
    }
}