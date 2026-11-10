namespace CoffeeManagement.DTO.Entities
{
    public sealed class UserSessionDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string EmployeeName { get; set; }
        public int? EmployeeId { get; set; }
    }
}
