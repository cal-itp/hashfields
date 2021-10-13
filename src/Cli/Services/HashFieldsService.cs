using System;
using System.Linq;

using HashFields.Cli.Options;
using HashFields.Data;
using HashFields.Data.Csv;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HashFields.Cli.Services
{
    internal class HashFieldsService
    {
        private readonly DataOptions _dataOptions;

        private readonly IServiceProvider _services;

        private readonly OptionsOutputStreamProvider _optionsStreamService;

        public HashFieldsService(
            IOptions<DataOptions> dataOptions,
            IServiceProvider services,
            OptionsOutputStreamProvider optionsStreamService
            )
        {
            _dataOptions = dataOptions.Value;
            _services = services;
            _optionsStreamService = optionsStreamService;
        }

        public void Run()
        {
            var csv = ActivatorUtilities.CreateInstance<Csv>(_services, _dataOptions.Delimiter);
            var stringHasher = ActivatorUtilities.CreateInstance<StringHasher>(_services, _dataOptions.HashAlgorithm);

            var dropColumns = _dataOptions.Drop.ToArray();
            csv.Remove(dropColumns);

            var hashColumns = csv.Header.Except(_dataOptions.Skip).ToArray();
            csv.Apply(stringHasher.Hash, hashColumns);

            using var destination = _optionsStreamService.Get();
            csv.Write(destination);
        }
    }
}
