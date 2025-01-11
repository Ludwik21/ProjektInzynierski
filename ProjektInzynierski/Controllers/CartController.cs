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

        [HttpPost("finalize")]
        public async Task<IActionResult> FinalizeCart(Guid clientId, DateTime startDate, DateTime endDate)
        {
            var userId = GetCurrentUserId();
            await _cartService.FinalizeCart(userId, clientId, startDate, endDate);
            return Ok();
        }

        protected Guid GetCurrentUserId()
        {
            var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                throw new InvalidOperationException("User is not authenticated.");
            }

            return Guid.Parse(userIdClaim);
        }
    }
}
