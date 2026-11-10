namespace CoffeeManagement.DTO.Entities
{
    public sealed class OrderLineDto
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
    }
}
