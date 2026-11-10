namespace CoffeeManagement.DTO.Entities
{
    public sealed class OrderSplitLineDto
    {
        public long OrderDetailId { get; set; }
        public decimal Quantity { get; set; }
    }
}
