using HashFields.Cli.Options;
using HashFields.Cli.Services;
using HashFields.Data;
using HashFields.Data.Csv;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HashFields.Cli
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IColumnOperator, Csv>()
                            .AddSingleton<IStreamWriter, Csv>()
                            .AddSingleton<IStringHasher, StringHasher>()
                            .AddSingleton<OptionsInputStreamProvider>()
                            .AddSingleton<OptionsOutputStreamProvider>();

                    services.AddTransient<IFileService, FileService>()
                            .AddTransient<IStreamProvider, OptionsOutputStreamProvider>()
                            .AddTransient<IStreamProvider, OptionsInputStreamProvider>();

                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        services.AddTransient<IConsoleService, DevConsoleService>();
                    }
                    else
                    {
                        services.AddTransient<IConsoleService, ConsoleService>();
                    }

                    services.AddOptions<DataOptions>()
                            .Bind(context.Configuration.GetSection(DataOptions.ConfigurationSectionName))
                            .ValidateDataAnnotations();

                    services.AddOptions<StreamOptions>()
                            .Bind(context.Configuration.GetSection(StreamOptions.ConfigurationSectionName))
                            .ValidateDataAnnotations();

                    services.AddHostedService<Worker>();
                });
    }
}
