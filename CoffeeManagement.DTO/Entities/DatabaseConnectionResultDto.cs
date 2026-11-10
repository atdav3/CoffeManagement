namespace CoffeeManagement.DTO.Entities
{
    public sealed class DatabaseConnectionResultDto
    {
        public string ServerName { get; set; }
        public string InstanceName { get; set; }
        public string DatabaseName { get; set; }
        public bool UsersTableAvailable { get; set; }
        public bool UserCountProcedureAvailable { get; set; }
        public long? UserCount { get; set; }
    }
}
