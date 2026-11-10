using System.Data;
using CoffeeManagement.DAL.Repositories;

namespace CoffeeManagement.BUS.Services
{
    public sealed class DashboardService
    {
        private readonly DashboardRepository repository = new DashboardRepository();
        public DataSet GetSummary() { return repository.GetSummary(); }
    }
}
