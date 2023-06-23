using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelExpertsData.Data;

namespace TravelExpertsData
{
    public class BookingManager
    {
        public static List<Booking> GetCustomerBooking(TravelExpertsContext context, string cusUserName)
        {
            int customerId = CustomerManager.FindCustomer(cusUserName, context).CustomerId;
            List<Booking> customerBookings = null;
            customerBookings = context.Bookings.Include(p => p.Package).Where(c => c.CustomerId == customerId).ToList();
            return customerBookings;
        }
    }
}
