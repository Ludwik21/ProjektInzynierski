﻿using Microsoft.AspNetCore.Mvc;
using ProjektInzynierski.Models;
using ProjektInzynierski.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ProjektInzynierski.Controllers
{
    public class CartItemController : Controller
    {
        // Dodanie przedmiotu do koszyka
        [HttpPost]
        public IActionResult AddToCart(int equipmentId, string name, decimal pricePerDay)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var existingItem = cart.FirstOrDefault(i => i.EquipmentID == equipmentId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem { EquipmentID = equipmentId, Name = name, PricePerDay = pricePerDay, Quantity = 1 });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return Json(new { success = true, cartCount = cart.Count });
        }

        // Usuwanie przedmiotu z koszyka
        [HttpPost]
        public IActionResult RemoveFromCart(int equipmentId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var itemToRemove = cart.FirstOrDefault(i => i.EquipmentID == equipmentId);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        // Widok koszyka
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        // Przejście do rezerwacji
        [HttpPost]
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            if (!cart.Any())
            {
                TempData["Error"] = "Koszyk jest pusty. Dodaj przedmioty przed dokonaniem rezerwacji.";
                return RedirectToAction("Index");
            }

            TempData["Message"] = "Rezerwacja została pomyślnie utworzona!";
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Create", "Reservations");
        }
    }
}
