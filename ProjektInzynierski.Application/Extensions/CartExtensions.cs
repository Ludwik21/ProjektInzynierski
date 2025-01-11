using ProjektInzynierski.Application.Models.CartItem;

namespace ProjektInzynierski.Application.Extensions
{
    public static class CartExtensions
    {
        public static string GetTotalPricesSummary(this IEnumerable<CartItemDto> cartItems)
        {
            string summary = "";

            foreach(var currency in cartItems.Select(x => x.PricePerDayCurrency).Distinct())
            {
                summary = string.Concat(summary, $"{cartItems.Where(x => x.PricePerDayCurrency == currency).Sum(y => y.TotalPrice)} {currency};");
            }

            return summary;
        }
    }
}
