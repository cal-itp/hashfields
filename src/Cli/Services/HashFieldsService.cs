using System.Linq;

using HashFields.Cli.Options;
using HashFields.Data;

using Microsoft.Extensions.Options;

namespace HashFields.Cli.Services
{
    internal class HashFieldsService
    {
        private readonly IColumnOperator _columnOperator;

        private readonly IStreamWriter _streamWriter;

        private readonly IStringHasher _stringHasher;

        private readonly DataOptions _dataOptions;

        private readonly OptionsOutputStreamProvider _optionsStreamService;

        public HashFieldsService(
            IColumnOperator columnOperator,
            IStreamWriter streamWriter,
            IStringHasher stringHasher,
            IOptions<DataOptions> dataOptions,
            OptionsOutputStreamProvider optionsStreamService
            )
        {
            _columnOperator = columnOperator;
            _streamWriter = streamWriter;
            _stringHasher = stringHasher;

            _dataOptions = dataOptions.Value;
            _optionsStreamService = optionsStreamService;
        }

        public void Run()
        {
            _columnOperator.Remove(_dataOptions.Drop.ToArray());

            var hashColumns = _columnOperator.Header.Except(_dataOptions.Skip).ToArray();
            _columnOperator.Apply(_stringHasher.Hash, hashColumns);

            using var destination = _optionsStreamService.Get();
            _streamWriter.Write(destination);
        }
    }
}
