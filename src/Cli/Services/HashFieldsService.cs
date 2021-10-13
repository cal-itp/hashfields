using System;
using System.Linq;

using HashFields.Cli.Options;
using HashFields.Data;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HashFields.Cli.Services
{
    internal class HashFieldsService
    {
        private readonly IServiceProvider _services;

        private readonly DataOptions _dataOptions;

        private readonly OptionsOutputStreamProvider _optionsStreamService;

        public HashFieldsService(
            IServiceProvider services,
            IOptions<DataOptions> dataOptions,
            OptionsOutputStreamProvider optionsStreamService
            )
        {
            _services = services;

            _dataOptions = dataOptions.Value;
            _optionsStreamService = optionsStreamService;
        }

        public void Run()
        {
            var columnOperator = ActivatorUtilities.CreateInstance<IColumnOperator>(_services, _dataOptions.Delimiter);
            var stringHasher = ActivatorUtilities.CreateInstance<IStringHasher>(_services, _dataOptions.HashAlgorithm);
            var streamWriter = ActivatorUtilities.CreateInstance<IStreamWriter>(_services);

            var dropColumns = _dataOptions.Drop.ToArray();
            columnOperator.Remove(dropColumns);

            var hashColumns = columnOperator.Header.Except(_dataOptions.Skip).ToArray();
            columnOperator.Apply(stringHasher.Hash, hashColumns);

            using var destination = _optionsStreamService.Get();
            streamWriter.Write(destination);
        }
    }
}
