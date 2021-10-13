using System;
using System.Linq;

using HashFields.Cli.Options;
using HashFields.Data;
using HashFields.Data.Csv;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HashFields.Cli.Services
{
    internal class HashFieldsService
    {
        private readonly DataOptions _dataOptions;

        private readonly ILogger<HashFieldsService> _logger;

        private readonly IServiceProvider _services;

        private readonly OptionsOutputStreamProvider _optionsStreamService;

        public HashFieldsService(
            ILogger<HashFieldsService> logger,
            IOptions<DataOptions> dataOptions,
            IServiceProvider services,
            OptionsOutputStreamProvider optionsStreamService
            )
        {
            _logger = logger;
            _dataOptions = dataOptions.Value;
            _services = services;
            _optionsStreamService = optionsStreamService;
        }

        public void Run()
        {
            _logger.LogInformation("Starting");

            _logger.LogInformation("Create Csv using Delimiter: {{{}}}", _dataOptions.Delimiter);
            var csv = ActivatorUtilities.CreateInstance<Csv>(_services, _dataOptions.Delimiter);

            _logger.LogInformation("Create StringHasher using HashAlgorithm: {{{}}}", _dataOptions.HashAlgorithm);
            var stringHasher = ActivatorUtilities.CreateInstance<StringHasher>(_services, _dataOptions.HashAlgorithm);

            var dropColumns = _dataOptions.Drop.ToArray();
            _logger.LogInformation("Dropping columns: {{{}}}", String.Join(", ", dropColumns));
            csv.Remove(dropColumns);

            var hashColumns = csv.Header.Except(_dataOptions.Skip).ToArray();
            _logger.LogInformation("Hashing columns: {{{}}}", String.Join(", ", hashColumns));
            csv.Apply(stringHasher.Hash, hashColumns);

            _logger.LogInformation("Writing results");
            using var destination = _optionsStreamService.Get();
            csv.Write(destination);

            _logger.LogInformation("Finished");
        }
    }
}
