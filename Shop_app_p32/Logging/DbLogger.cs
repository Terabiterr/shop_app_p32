using Shop_app_p32.Models;

namespace Shop_app_p32.Logging
{
    public class DbLogger : ILogger
    {
        public readonly IServiceProvider _serviceProvider;
        public DbLogger(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null!;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var message = formatter(state, exception);
            if (string.IsNullOrEmpty(message)) return;
            using var scope = _serviceProvider.CreateScope();
            var db_context = scope.ServiceProvider.GetRequiredService<ShopContext>();
            var logEntry = new Log
            {
                Message = formatter(state, exception),
                Level = logLevel.ToString(),
                Timestamp = DateTime.UtcNow,
                Exception = exception?.ToString() ?? exception?.StackTrace
            };

            db_context.Logs.Add(logEntry);
            db_context.SaveChanges();
        }
    }
}
