namespace CoffeeManagement.DTO.Entities
{
    public sealed class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
    }
}
