using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelExpertsData.Data;

namespace TravelExpertsData
{
    public class CustomerManager
    {
        /// <summary>
        /// User is authenticated based on credentials and a user returned if exists or null if not.
        /// </summary>
        /// <param name="username">Username as string</param>
        /// <param name="password">Password as string</param>
        /// <returns>A user object or null.</returns>
        /// <remarks>
        /// Add additional for the docs for this application--for developers.
        /// </remarks>
        public static Customer Authenticate(string username, string password, TravelExpertsContext db)
        {
            Customer cust = null;
            cust = db.Customers.SingleOrDefault(c => c.Username == username && c.Password == password);

            return cust; //this will either be null or an object
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="db"></param>
        /// <returns>customer or null</returns>
        public static Customer FindCustomer(string username, TravelExpertsContext db)
        {

            Customer cust = null;

            cust = db.Customers.Include(b => b.Bookings).SingleOrDefault(c => c.Username == username); //get customer with including associated

            return cust;
        }

        
    }
}
