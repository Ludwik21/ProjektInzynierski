using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjektInzynierski.Application.Models.CartItem;
using ProjektInzynierski.Domain.Entities.CartItem;

namespace ProjektInzynierski.Application.Mappers
{
        public static class CartMapper
        {
            public static CartItemDto ToDto(CartItem item)
            {
                return new CartItemDto
                {
                    EquipmentId = item.EquipmentId,
                    Name = item.Name,
                    PricePerDay = item.PricePerDay,
                    PricePerDayCurrency = item.PricePerDayCurrency,
                    Quantity = item.Quantity
                };
            }

            public static CartDto ToDto(IEnumerable<CartItem> items)
            {
                return new CartDto
                {
                    Items = items.Select(ToDto).ToList()
                };
            }
        }
}
