using System;
using CoffeeManagement.DAL.Database;

namespace CoffeeManagement.BUS.Services
{
    public sealed class BusinessCalendarService
    {
        public DateTime Today { get { return BusinessTime.Today; } }
    }
}
