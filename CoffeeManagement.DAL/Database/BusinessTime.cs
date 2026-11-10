using System;
using System.Configuration;

namespace CoffeeManagement.DAL.Database
{
    public static class BusinessTime
    {
        private static readonly Lazy<TimeZoneInfo> Zone = new Lazy<TimeZoneInfo>(() =>
        {
            string id = ConfigurationManager.AppSettings["BusinessTimeZoneId"];
            return TimeZoneInfo.FindSystemTimeZoneById(string.IsNullOrWhiteSpace(id) ? "SE Asia Standard Time" : id);
        });

        public static DateTime Today
        {
            get { return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Zone.Value).Date; }
        }

        public static DateTime ToUtc(DateTime localDateTime)
        {
            DateTime local = DateTime.SpecifyKind(localDateTime, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(local, Zone.Value);
        }
    }
}
