using CleanArchitectureApi.Application.Contracts.Infrastructure;

namespace CleanArchitectureApi.Infrastructure.Services;

/// <summary>
/// Service for providing date and time functionality
/// </summary>
public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.UtcNow;

    public DateTime Today => DateTime.UtcNow.Date;

    public TimeZoneInfo LocalTimeZone => TimeZoneInfo.Local;

    public DateTime ConvertToTimeZone(DateTime dateTime, TimeZoneInfo timeZone)
    {
        return TimeZoneInfo.ConvertTime(dateTime, timeZone);
    }

    public DateTime ConvertFromUtc(DateTime utcDateTime, TimeZoneInfo timeZone)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZone);
    }

    public DateTime ConvertToUtc(DateTime dateTime, TimeZoneInfo timeZone)
    {
        return TimeZoneInfo.ConvertTimeToUtc(dateTime, timeZone);
    }

    // TODO: Add additional date/time utility methods as needed
    // public bool IsWeekend(DateTime date)
    // {
    //     return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    // }

    // public DateTime AddBusinessDays(DateTime date, int days)
    // {
    //     var result = date;
    //     for (int i = 0; i < days; i++)
    //     {
    //         result = result.AddDays(1);
    //         while (IsWeekend(result))
    //         {
    //             result = result.AddDays(1);
    //         }
    //     }
    //     return result;
    // }
}