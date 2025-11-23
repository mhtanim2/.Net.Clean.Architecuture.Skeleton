using System.Reflection;

namespace CleanArchitectureApi.Infrastructure.Services;

/// <summary>
/// Service for providing system information
/// </summary>
public class SystemInfoService : ISystemInfoService
{
    public string GetApplicationVersion()
    {
        return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";
    }

    public string GetMachineName()
    {
        return Environment.MachineName;
    }

    public string GetEnvironmentName()
    {
        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown";
    }

    public string GetOperatingSystem()
    {
        return Environment.OSVersion.ToString();
    }

    public int GetProcessorCount()
    {
        return Environment.ProcessorCount;
    }

    public long GetWorkingSet()
    {
        return GC.GetTotalMemory(false);
    }

    public string GetRuntimeVersion()
    {
        return Environment.Version.ToString();
    }

    // TODO: Add additional system info methods as needed
    // public string GetApplicationName()
    // {
    //     return Assembly.GetExecutingAssembly().GetName().Name ?? "CleanArchitectureApi";
    // }

    // public DateTime GetApplicationStartTime()
    // {
    //     using var process = System.Diagnostics.Process.GetCurrentProcess();
    //     return process.StartTime;
    // }
}