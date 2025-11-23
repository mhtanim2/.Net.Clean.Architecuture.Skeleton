namespace CleanArchitectureApi.Application.Contracts.Infrastructure;

/// <summary>
/// Service interface for system information
/// </summary>
public interface ISystemInfoService
{
    string GetApplicationVersion();
    string GetMachineName();
    string GetEnvironmentName();
    string GetOperatingSystem();
    int GetProcessorCount();
    long GetWorkingSet();
    string GetRuntimeVersion();

    // TODO: Add additional method signatures as needed
    // string GetApplicationName();
    // DateTime GetApplicationStartTime();
}