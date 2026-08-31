using Microsoft.Extensions.Logging;

namespace Webapia.TestCommon;

public class TestLogger<T> : ILogger<T>
{
    public List<(LogLevel Level, Exception? Exception, string Message)> Logs { get; } = new();

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        Logs.Add((logLevel, exception, formatter(state, exception)));
    }
}