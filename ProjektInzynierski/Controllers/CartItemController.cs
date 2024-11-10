using Microsoft.AspNetCore.Mvc;
using ProjektInzynierski.Models;
using ProjektInzynierski.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ProjektInzynierski.Controllers
{
    public class CartItemController : Controller
    {
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        public IActionResult AddToCart(int equipmentId, string name, decimal pricePerDay)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var item = cart.FirstOrDefault(i => i.EquipmentID == equipmentId);
            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                cart.Add(new CartItem { EquipmentID = equipmentId, Name = name, PricePerDay = pricePerDay, Quantity = 1 });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int equipmentId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var item = cart.FirstOrDefault(i => i.EquipmentID == equipmentId);

            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
    }
}
