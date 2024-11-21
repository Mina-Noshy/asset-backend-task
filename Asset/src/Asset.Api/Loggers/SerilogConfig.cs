using Asset.Domain.Utilities;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace Asset.Api.Loggers;

public static class SerilogConfig
{
    public static void EnsureInitialized()
    {
        string filePath = ConfigurationHelper.GetSerilog("path")
            .Replace("{date}", DateTime.Now.ToString("yyyyMMddHHmmss"));

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(filePath)
            .WriteTo.MSSqlServer(ConfigurationHelper.GetConnectionString(),
                        new MSSqlServerSinkOptions
                        {
                            TableName = ConfigurationHelper.GetSerilog("tableName"),
                            SchemaName = ConfigurationHelper.GetSerilog("schemaName"),
                            AutoCreateSqlTable = bool.Parse(ConfigurationHelper.GetSerilog("autoCreateSqlTable"))
                        })
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .CreateLogger();
    }
}
