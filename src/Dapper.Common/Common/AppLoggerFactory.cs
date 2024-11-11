using Microsoft.Extensions.Logging;

namespace Dapper.Common.Common;

public static class AppLoggerFactory
{
    private static ILoggerFactory _loggerFactory = default!; /*= new LoggerFactory();*/
    
    public static void SetLoggerFactory(ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);
        _loggerFactory = loggerFactory;
    }

    public static ILogger<T> CreateLogger<T>() => _loggerFactory.CreateLogger<T>();        
    
    public static ILogger CreateLogger(string categoryName) => _loggerFactory.CreateLogger(categoryName);
}