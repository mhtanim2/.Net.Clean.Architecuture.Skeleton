namespace CleanArchitectureApi.Application.Contracts.Infrastructure;

/// <summary>
/// Service interface for date and time operations
/// </summary>
public interface IDateTimeService
{
    DateTime Now { get; }
    DateTime Today { get; }
    TimeZoneInfo LocalTimeZone { get; }

    DateTime ConvertToTimeZone(DateTime dateTime, TimeZoneInfo timeZone);
    DateTime ConvertFromUtc(DateTime utcDateTime, TimeZoneInfo timeZone);
    DateTime ConvertToUtc(DateTime dateTime, TimeZoneInfo timeZone);

    // TODO: Add additional method signatures as needed
    // bool IsWeekend(DateTime date);
    // DateTime AddBusinessDays(DateTime date, int days);
}