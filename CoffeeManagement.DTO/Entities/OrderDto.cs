using System.Collections.Generic;

namespace CoffeeManagement.DTO.Entities
{
    public sealed class OrderDto
    {
        public long? OrderId { get; set; }
        public int? TableId { get; set; }
        public int EmployeeId { get; set; }
        public int? CustomerId { get; set; }
        public int? PromotionId { get; set; }
        public string Note { get; set; }
        public IList<OrderLineDto> Lines { get; set; } = new List<OrderLineDto>();
    }
}
