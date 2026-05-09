namespace Shop_app_p32.Logging
{
    public class DbLoggerProvider : ILoggerProvider
    {
        private readonly IServiceProvider _serviceProvider;
        public DbLoggerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(_serviceProvider);
        }

        public void Dispose()
        {

        }
    }
}
