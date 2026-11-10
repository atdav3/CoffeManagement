using System;

namespace CoffeeManagement.DTO.Entities
{
    public sealed class PromotionDto
    {
        public int PromotionId { get; set; }
        public string PromotionName { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
    }
}
