using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using web.Data;
using web.Models;
using Type = web.Models.Type;

namespace web.Controllers
{
    public class BookingsController : Controller
    {
        private readonly RentContext _context;

        public BookingsController(RentContext context)
        {
            _context = context;
        }

        // GET: All Bookings
        public async Task<IActionResult> Index(string? id)
        {
            var rentContext = id == null
                ? _context.Bookings.Include(b => b.Car).Include(b => b.Customer)
                : _context.Bookings.Include(b => b.Car).Where(b => b.Customer.UserID == id).Include(b => b.Customer);
            return View(await rentContext.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Car)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(m => m.BookingID == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
        public static List<DateTime> GetDates(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                .Select(day => new DateTime(year, month, day)) // Map each day to a date
                .ToList(); // Load dates into a list
        }


        // GET: Bookings/Create
        public IActionResult Create(int CarID, string UserID) {
            Car car = _context.Cars.Find(CarID);
            List<DateTime> allDates = new List<DateTime>();
            int currYear = DateTime.Now.Year;
            int currMonth = DateTime.Now.Month;
            for (int month = 0; month <= 12; month++)
                allDates.AddRange(GetDates(currMonth + month <= 12 ? currYear : currYear + 1, (currMonth + month) > 12 ? ((currMonth + month) % 12) + 1 : currMonth + month));
            int customerID = _context.Customers
                .Where(c => c.UserID == UserID)
                .Select(c => c.CustomerID).FirstOrDefault();
            ViewData["CustomerID"] = customerID;
            ViewData["CarID"] = new SelectListItem{Value = CarID.ToString()};
            ViewData["City"] = _context.Cars
                .Where(c => c.Color == car.Color && c.Model == car.Model && c.Make == car.Make)
                .Select(c => new SelectListItem{Value = c.Location.City, Text = c.Location.City.ToString()})
                .Distinct();
            ViewData["Address"] = _context.Cars
                .Where(c => c.Color == car.Color && c.Model == car.Model && c.Make == car.Make)
                .Select(c => new SelectListItem{Value = c.Location.Address, Text = c.Location.Address.ToString()})
                .Distinct();
            string[] types = {"Card", "Cash", "Crypto"};
            ViewData["Types"] = types
                .Select(c => new SelectListItem { Value = c.ToString(), Text = c.ToString() });
            var occupiedDates = _context.Bookings
                            .Where(c => c.Car.Color == car.Color && c.Car.Model == car.Model && c.Car.Make == car.Make)
                            .Select(c => new {c.startDate, c.endDate}).ToList();
            ViewData["Dates"] = allDates
                            .Where(date => !occupiedDates.Any(booking => date >= booking.startDate && date <= booking.endDate));

return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int customerID, string City, string Address, DateTime startDate, DateTime endDate, Type type) {
            int carID = _context.Cars
                .Where(c => c.Location.City == City && c.Location.Address == Address)
                .Select(c => c.CarID)
                .FirstOrDefault();
            
            Booking booking = new Booking{CustomerID = customerID, CarID = carID, startDate = startDate, endDate = endDate};
            
            int dailyRate = _context.Cars.Find(carID).DailyRate;
            
            if (ModelState.IsValid){
                string userID = _context.Customers
                    .Where(c => c.CustomerID == booking.CustomerID)
                    .Select(c => c.UserID)
                    .FirstOrDefault();
                _context.Add(booking);
                await _context.SaveChangesAsync();
                Payment payment = new Payment{BookingID = booking.BookingID, PaymentMethod = type, PaymentDate = DateTime.Now, PayAmount = endDate.Subtract(startDate).Days * dailyRate, Paid = true};
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Bookings", new { id = userID });
            }
            else {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors)) {
                    Console.WriteLine($"Error: {modelError.ErrorMessage}");
                }
            }
/*            ViewData["CarID"] = new SelectList(_context.Cars, "CarID", "CarID", booking.CarID);
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", booking.CustomerID);*/
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            ViewData["CarID"] = new SelectList(_context.Cars, "CarID", "CarID", booking.CarID);
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", booking.CustomerID);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("BookingID,CarID,CustomerID,LocationID,startDate,endDate")] Booking booking)
        {
            if (id != booking.BookingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CarID"] = new SelectList(_context.Cars, "CarID", "CarID", booking.CarID);
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", booking.CustomerID);
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
                .Include(b => b.Car)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(m => m.BookingID == id);
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
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'RentContext.Bookings'  is null.");
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");;
        }

        private bool BookingExists(int id)
        {
            return (_context.Bookings?.Any(e => e.BookingID == id)).GetValueOrDefault();
        }
    }
}