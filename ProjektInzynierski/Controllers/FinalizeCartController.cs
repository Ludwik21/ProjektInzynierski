using Microsoft.AspNetCore.Mvc;
using ProjektInzynierski.Application.Models;
using ProjektInzynierski.Application.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektInzynierski.Controllers
{
    public class FinalizeReservationController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IReservationService _reservationService;

        public FinalizeReservationController(ICartService cartService, IReservationService reservationService)
        {
            _cartService = cartService;
            _reservationService = reservationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new FinalizeCartModel
            {
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date.AddDays(1) // Domyślnie następny dzień
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult SetDates(FinalizeCartModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid dates. Please try again.";
                return RedirectToAction("Index", "Cart");
            }

            // Walidacja dat
            if (model.StartDate < DateTime.Now.Date)
            {
                TempData["ErrorMessage"] = "Start date cannot be in the past.";
                return RedirectToAction("Index", "Cart");
            }

            if (model.EndDate <= model.StartDate)
            {
                TempData["ErrorMessage"] = "End date must be after the start date.";
                return RedirectToAction("Index", "Cart");
            }

            TempData["StartDate"] = model.StartDate;
            TempData["EndDate"] = model.EndDate;

            return RedirectToAction("Index", "Cart");
        }

    }
}