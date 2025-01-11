using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektInzynierski.Application.Mappers;
using ProjektInzynierski.Application.Services;
using System.Security.Claims;

namespace ProjektInzynierski.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Pobranie danych koszyka jako DTO
            var cartItems = _cartService.GetCartItemsAsDto();

            // Obliczenie łącznej kwoty i przekazanie jej do widoku
            ViewBag.TotalPrice = cartItems.Sum(item => item.TotalPrice);

            // Przekazanie danych DTO do widoku
            return View(cartItems);
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid equipmentId)
        {
            // Dodanie produktu do koszyka za pomocą serwisu
            await _cartService.AddToCart(equipmentId);

            // Przekierowanie po dodaniu do koszyka
            return RedirectToAction("Index", "Equipments"); // Lub "Cart", jeśli chcesz od razu pokazać koszyk
        }


        [HttpPost]
        public IActionResult RemoveFromCart(Guid equipmentId)
        {
            _cartService.RemoveFromCart(equipmentId);
            return RedirectToAction("Index");
        }

        [HttpPost("update")]
        public IActionResult UpdateCartItem(Guid equipmentId, int quantity)
        {
            _cartService.UpdateCartItem(equipmentId, quantity);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> FinalizeCart()
        {
            var userName = GetUserName();
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            await _cartService.FinalizeCart(userName, startDate, endDate);
            return Ok();
        }

        protected string GetUserName()
        {
            var userName = User?.FindFirst(ClaimTypes.Name)?.Value;

            if (userName == null)
            {
                throw new InvalidOperationException("User is not authenticated.");
            }

            return userName;
        }
    }
}
