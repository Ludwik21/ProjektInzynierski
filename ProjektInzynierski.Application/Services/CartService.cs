using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProjektInzynierski.Application.Extensions;
using ProjektInzynierski.Application.Models;
using ProjektInzynierski.Infrastructure.Repositories;
using ProjektInzynierski.Domain.Entities.CartItem;
using ProjektInzynierski.Domain.Entities.Reservations;
using ProjektInzynierski.Application.Models.CartItem;

namespace ProjektInzynierski.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IReservationRepository _reservationRepository;
        private const string CartSessionKey = "Cart";

        public CartService(IHttpContextAccessor httpContextAccessor, IReservationRepository reservationRepository)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
        }

        private ISession Session => _httpContextAccessor.HttpContext?.Session
            ?? throw new InvalidOperationException("Session is not available in the current context.");

        public void AddToCart(Guid equipmentId, string equipmentName, decimal price)
        {
            // Pobierz istniejący koszyk z sesji
            var cart = GetCartFromSession();

            // Sprawdź, czy produkt już istnieje w koszyku
            var item = cart.FirstOrDefault(c => c.EquipmentId == equipmentId);

            if (item != null)
            {
                // Jeśli produkt istnieje, zwiększ ilość
                item.Quantity++;
            }
            else
            {
                // Dodaj nowy produkt do koszyka
                cart.Add(new CartItem
                {
                    EquipmentId = equipmentId,
                    Name = equipmentName,
                    PricePerDay = price,
                    Quantity = 1
                });
            }

            // Zapisz koszyk w sesji
            SaveCartToSession(cart);
        }

        public void RemoveFromCart(Guid equipmentId)
        {
            var cart = GetCartFromSession();
            cart.RemoveAll(c => c.EquipmentId == equipmentId);
            SaveCartToSession(cart);
        }

        public void UpdateCartItem(Guid equipmentId, int quantity)
        {
            var cart = GetCartFromSession();
            var item = cart.FirstOrDefault(c => c.EquipmentId == equipmentId);
            if (item != null)
            {
                item.Quantity = quantity;
            }
            SaveCartToSession(cart);
        }

        public IEnumerable<CartItem> GetCartItems() => GetCartFromSession();

        public decimal GetTotalPrice() => GetCartFromSession().Sum(c => c.PricePerDay * c.Quantity);

        public async Task FinalizeCart(Guid userId, Guid clientId, DateTime startDate, DateTime endDate)
        {
            var cart = GetCartFromSession();
            if (!cart.Any()) return;

            var reservation = new Reservation(
                userId,
                clientId,
                startDate,
                endDate
            );

            foreach (var item in cart)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    reservation.AddEquipment(item.EquipmentId);
                }
            }

            await _reservationRepository.AddReservation(reservation);
            ClearCart();
        }

        public IEnumerable<CartItemDto> GetCartItemsAsDto()
        {
            var cartItems = GetCartFromSession();

            return cartItems.Select(item => new CartItemDto
            {
                EquipmentId = item.EquipmentId,
                Name = item.Name,
                PricePerDay = item.PricePerDay,
                Quantity = item.Quantity
            });
        }


        private List<CartItem> GetCartFromSession()
        {
            var cart = Session.GetObjectFromJson<List<CartItem>>(CartSessionKey);
            return cart ?? new List<CartItem>();
        }

        private void SaveCartToSession(List<CartItem> cart)
        {
            Session.SetObjectAsJson(CartSessionKey, cart);
        }

        private void ClearCart()
        {
            Session.Remove(CartSessionKey);
        }
    }
}
