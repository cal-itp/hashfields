using System;
using System.Threading;
using System.Threading.Tasks;

using HashFields.Cli.Services;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HashFields.Cli
{
    internal class Worker : IHostedService
    {
        private readonly HashFieldsService _hashFields;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ILogger<Worker> _logger;

        public Worker(
            HashFieldsService hashFields,
            IHostApplicationLifetime appLifetime,
            ILogger<Worker> logger)
        {
            _hashFields = hashFields ?? throw new ArgumentNullException(nameof(hashFields));
            _appLifetime = appLifetime ?? throw new ArgumentNullException(nameof(appLifetime));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task task;

            if (cancellationToken.IsCancellationRequested)
            {
                task = Task.FromCanceled(cancellationToken);
            }
            else
            {
                _logger.LogInformation("Hello HashFields.Cli");

                try
                {
                    _hashFields.Run();
                    task = Task.CompletedTask;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "HashFields.Cli had a problem");
                    task = Task.FromException(ex);
                }
                finally
                {
                    _appLifetime.StopApplication();
                }
            }

            return task;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            _logger.LogInformation("Goodbye HashFields.Cli");

            return Task.CompletedTask;
        }
    }
}
