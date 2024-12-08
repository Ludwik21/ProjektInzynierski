//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace ProjektInzynierski.Controllers
//{
//    public class PaymentsController : Controller
//    {

//        // GET: Payments
//        public async Task<IActionResult> Index()
//        {
//            return View(await new List<>);
//        }

//        // GET: Payments/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var payment = await _context.Payments
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (payment == null)
//            {
//                return NotFound();
//            }

//            return View(payment);
//        }

//        // GET: Payments/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: Payments/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("PaymentID,ReservationID,Amount,PaymentMethod,PaymentDate")] Payment payment)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(payment);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(payment);
//        }

//        // GET: Payments/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var payment = await _context.Payments.FindAsync(id);
//            if (payment == null)
//            {
//                return NotFound();
//            }
//            return View(payment);
//        }

//        // POST: Payments/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("PaymentID,ReservationID,Amount,PaymentMethod,PaymentDate")] Payment payment)
//        {
//            if (id != payment.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(payment);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!PaymentExists(payment.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View(payment);
//        }

//        // GET: Payments/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var payment = await _context.Payments
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (payment == null)
//            {
//                return NotFound();
//            }

//            return View(payment);
//        }

//        // POST: Payments/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var payment = await _context.Payments.FindAsync(id);
//            if (payment != null)
//            {
//                _context.Payments.Remove(payment);
//                await _context.SaveChangesAsync();
//            }
//            return RedirectToAction(nameof(Index));
//        }
//        public async Task<IActionResult> ConfirmReservation(int id)
//        {
//            var reservation = await _context.Reservations.FindAsync(id);
//            if (reservation == null)
//            {
//                return NotFound();
//            }

//            // Oznacz rezerwację jako potwierdzoną (status)
//            reservation.Status = "Confirmed";
//            await _context.SaveChangesAsync();

//            // Przekierowanie do płatności
//            return RedirectToAction("Create", "Payments", new { reservationId = id });
//        }

//        private bool PaymentExists(int id)
//        {
//            return _context.Payments.Any(e => e.Id == id);
//        }
//    }
//}
