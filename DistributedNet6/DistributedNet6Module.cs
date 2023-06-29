using Serilog;
using Serilog.Events;

namespace DistributedNet6;

public static class DistributedNet6Module
{
    public static void AddSerilLog(this ConfigureHostBuilder configureHostBuilder)
    {
        string outputTemplate="{NewLine}【{Level:u3}】{Timestamp:yyyy-MM-dd HH:mm:ss.fff}" +
            "{NewLine}#Msg#{Message:lj}" +
            "{NewLine}#Pro #{Properties:j}" +
            "{NewLine}#Exc#{Exception}" +
            new string('-', 50);
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: outputTemplate)
            .WriteTo.Logger(configure =>
                configure.MinimumLevel.Debug()
                    .WriteTo.File($"logs/log.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: outputTemplate)
            ).CreateLogger();
        configureHostBuilder.UseSerilog(Log.Logger);
    }
}