using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace RestEaseXmlExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create ServiceCollection
            var services = new ServiceCollection();

            // Add logging & services
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddConsole();
                builder.AddDebug();
            });
            services.AddSingleton<IXmlSerializer, XmlSerializer>();
            services.AddSingleton<IXmlRequestBodySerializer, XmlRequestBodySerializer>();

            // https://www.stevejgordon.co.uk/introduction-to-httpclientfactory-aspnetcore
            services.AddHttpClient();

            // register the ISomeApiClientFactory
            services.AddSingleton<ISomeApiClientFactory, SomeApiClientFactory>();

            var serviceProvider = services.BuildServiceProvider();

            // Resolve ILoggerFactory and ILogger via DI
            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
            logger.LogInformation("Going to send XML...");

            // Resolve via DI
            var factory = serviceProvider.GetService<ISomeApiClientFactory>();
            var api = factory.CreateClient<ISomeApi>();
            api.Endpoint = "http://postman-echo.com/post";

            var result = api.PostSettingsAsync(new TestModel { Id = 5, X = "abc" }).GetAwaiter().GetResult();
            Console.WriteLine(result);
        }
    }
}
