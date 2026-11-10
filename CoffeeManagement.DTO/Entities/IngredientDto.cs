namespace CoffeeManagement.DTO.Entities
{
    public sealed class IngredientDto
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public int UnitId { get; set; }
        public decimal ReorderLevel { get; set; }
    }
}
