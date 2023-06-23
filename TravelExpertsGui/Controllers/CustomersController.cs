using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelExpertsData;
using TravelExpertsData.Data;

namespace TravelExpertsGui.Controllers
{
    public class CustomersController : Controller
    {
        private readonly TravelExpertsContext _context;

        public CustomersController(TravelExpertsContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var travelExpertsContext = _context.Customers.Include(c => c.Agent);
            return View(await travelExpertsContext.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Agent)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["AgentId"] = new SelectList(_context.Agents, "AgentId", "1");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,CustFirstName,CustLastName,CustAddress,CustCity,CustProv,CustPostal,CustCountry,CustHomePhone,CustBusPhone,CustEmail,AgentId,Username,Password,ConfirmPassword")] Customer customer)
        {
            if(CustomerExists(customer.Username))
            {
                TempData["Message"] = "Username already exist.";
                TempData["IsError"] = true;
                return View(customer);
            }
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
            }
            //ViewData["AgentId"] = new SelectList(_context.Agents, "AgentId", "AgentId", customer.AgentId);
            return RedirectToAction("Login", "Account");
        }

        //// GET: Customers/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Customers == null)
        //    {
        //        return NotFound();
        //    }

        //    var customer = await _context.Customers.FindAsync(id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["AgentId"] = new SelectList(_context.Agents, "AgentId", "AgentId", customer.AgentId);
        //    return View(customer);
        //}

        //// POST: Customers/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("CustomerId,CustFirstName,CustLastName,CustAddress,CustCity,CustProv,CustPostal,CustCountry,CustHomePhone,CustBusPhone,CustEmail,AgentId,Username,Password")] Customer customer)
        //{
        //    if (id != customer.CustomerId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(customer);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CustomerExists(customer.CustomerId))
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
        //    ViewData["AgentId"] = new SelectList(_context.Agents, "AgentId", "AgentId", customer.AgentId);
        //    return View(customer);
        //}

        // GET: Customers/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Customers == null)
        //    {
        //        return NotFound();
        //    }

        //    var customer = await _context.Customers
        //        .Include(c => c.Agent)
        //        .FirstOrDefaultAsync(m => m.CustomerId == id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(customer);
        //}

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Customers == null)
        //    {
        //        return Problem("Entity set 'TravelExpertsContext.Customers'  is null.");
        //    }
        //    var customer = await _context.Customers.FindAsync(id);
        //    if (customer != null)
        //    {
        //        _context.Customers.Remove(customer);
        //    }
            
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool CustomerExists(string username)
        {
          return (_context.Customers?.Any(e => e.Username == username)).GetValueOrDefault();
        }
    }
}
