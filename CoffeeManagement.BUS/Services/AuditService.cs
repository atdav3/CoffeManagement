using System;
using System.Data;
using CoffeeManagement.BUS.Exceptions;
using CoffeeManagement.DAL.Repositories;

namespace CoffeeManagement.BUS.Services
{
    public sealed class AuditService
    {
        private readonly AuditRepository repository = new AuditRepository();
        public DataTable Get(DateTime start, DateTime end)
        {
            if (start.Date > end.Date) throw new BusinessException("Start date must be on or before end date.");
            return repository.Get(start.Date, end.Date.Date.AddDays(1));
        }
    }
}
