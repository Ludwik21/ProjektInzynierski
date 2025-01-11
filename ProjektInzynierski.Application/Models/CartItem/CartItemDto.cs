﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektInzynierski.Application.Models.CartItem
{
        public class CartItemDto
        {
            public Guid EquipmentId { get; set; }
            public string Name { get; set; }
            public decimal PricePerDay { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice => PricePerDay * Quantity;
        }
}
