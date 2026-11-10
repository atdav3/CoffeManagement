using System;
using System.Data;
using CoffeeManagement.BUS.Exceptions;
using CoffeeManagement.DAL.Repositories;
using CoffeeManagement.DTO.Enums;

namespace CoffeeManagement.BUS.Services
{
    public sealed class ReportService
    {
        private readonly ReportRepository repository = new ReportRepository();
        public DataTable Get(ReportKind kind, DateTime start, DateTime end)
        {
            if (start.Date > end.Date) throw new BusinessException("Start date must be on or before end date.");
            return repository.Get(kind, start.Date, end.Date.Date.AddDays(1));
        }
    }
}
