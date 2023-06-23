using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelExpertsData;
using TravelExpertsData.Data;

namespace TravelExpertsGui.Controllers
{
    public class BookingsController : Controller
    {
        private readonly TravelExpertsContext _context;
        public BookingsController(TravelExpertsContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            if (User.Identity.Name != null)
            {
                decimal totalbookingCost = 0; //this is to hold the total cost of the customer bookings
                int customerId = CustomerManager.FindCustomer(User.Identity.Name, _context).CustomerId;
                var travelExpertsContext = _context.Bookings.Include(b => b.CustomerId == customerId).Include(b => b.Package).Include(b => b.TripType);
                List<Booking> custBooking = null;
                custBooking = _context.Bookings.Include(p => p.Package).Include(t => t.TripType).Where(c => c.CustomerId == customerId).ToList();
                foreach (var c in custBooking)
                {
                    if (c.PackageId != null)
                    {
                        totalbookingCost += CalTotalCost(c);
                        //c.Package.PkgBasePrice += (decimal)c.Package.PkgAgencyCommission;
                    }
                };             
                ViewBag.TotalCost = totalbookingCost.ToString("c");
                return View(custBooking);
            }
            else
            {
                return View("Index", "Home");
            }
        }
        /// <summary>
        /// Calculates the total booking cost
        /// </summary>
        /// <param name="c">Booking object</param>
        /// <returns>returns total booking cost as a decimal</returns>

        private decimal CalTotalCost(Booking c)
        {
             return (c.Package.PkgBasePrice * Convert.ToDecimal(c.TravelerCount));
        }


        // GET: Bookings/Create
        public IActionResult Create(int Id)
        {
            if(User.Identity.Name != null)
            {
                
                int custId = CustomerManager.FindCustomer(User.Identity.Name, _context).CustomerId;
                List<TripType> tripTypes = TripTypeManager.GetTripTypes(_context);
                var list = new SelectList(tripTypes, "TripTypeId", "Ttname").ToList();
                ViewBag.BookingNum = custId * 100 +  DateTime.Now.Second + User.Identity.Name;
                ViewBag.CustomerId = custId;
                ViewBag.PackageId = Id;
                ViewBag.TripType = list;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,BookingDate,BookingNo,TravelerCount,CustomerId,TripTypeId,PackageId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustAddress", booking.CustomerId);
            ViewData["PackageId"] = new SelectList(_context.Packages, "PackageId", "PkgName", booking.PackageId);
            ViewData["TripTypeId"] = new SelectList(_context.TripTypes, "TripTypeId", "TripTypeId", booking.TripTypeId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Package)
                .Include(b => b.TripType)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (_context.Bookings == null)
                {
                    return Problem("Entity set 'TravelExpertsContext.Bookings'  is null.");
                }
                var booking = await _context.Bookings.FindAsync(id);
                if (booking != null)
                {
                    _context.Bookings.Remove(booking);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Message"] = "Can't delete this booking. It is referenced in the database.";
                TempData["IsError"] = true;
            }

            return View("index");
        }

        // GET: Bookings/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Bookings == null)
        //    {
        //        return NotFound();
        //    }

        //    var booking = await _context.Bookings
        //        .Include(b => b.Customer)
        //        .Include(b => b.Package)
        //        .Include(b => b.TripType)
        //        .FirstOrDefaultAsync(m => m.BookingId == id);
        //    if (booking == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(booking);
        //}

        //// GET: Bookings/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Bookings == null)
        //    {
        //        return NotFound();
        //    }

        //    var booking = await _context.Bookings.FindAsync(id);
        //    if (booking == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustAddress", booking.CustomerId);
        //    ViewData["PackageId"] = new SelectList(_context.Packages, "PackageId", "PkgName", booking.PackageId);
        //    ViewData["TripTypeId"] = new SelectList(_context.TripTypes, "TripTypeId", "TripTypeId", booking.TripTypeId);
        //    return View(booking);
        //}

        //// POST: Bookings/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("BookingId,BookingDate,BookingNo,TravelerCount,CustomerId,TripTypeId,PackageId")] Booking booking)
        //{
        //    if (id != booking.BookingId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(booking);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BookingExists(booking.BookingId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustAddress", booking.CustomerId);
        //    ViewData["PackageId"] = new SelectList(_context.Packages, "PackageId", "PkgName", booking.PackageId);
        //    ViewData["TripTypeId"] = new SelectList(_context.TripTypes, "TripTypeId", "TripTypeId", booking.TripTypeId);
        //    return View(booking);
        //}



        //private bool BookingExists(int id)
        //{
        //  return (_context.Bookings?.Any(e => e.BookingId == id)).GetValueOrDefault();
        //}
    }
}
