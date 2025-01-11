using ProjektInzynierski.Domain.Entities.CartItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjektInzynierski.Application.Models.CartItem;

namespace ProjektInzynierski.Application.Services
{
    public interface ICartService
    {
        IEnumerable<CartItemDto> GetCartItemsAsDto();
        Task AddToCart(Guid equipmentId);
        void RemoveFromCart(Guid equipmentId);
        void UpdateCartItem(Guid equipmentId, int quantity);
        Task FinalizeCart(string userName, DateTime startDate, DateTime endDate);
    }
}

