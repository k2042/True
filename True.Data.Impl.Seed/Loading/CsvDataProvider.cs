using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.Text;
using True.Data.Model.Seed;

namespace True.Data.Impl.Seed.Loading
{
    public class CsvDataProvider<TSeedEntity> : ISeedDataProvider<TSeedEntity>
        where TSeedEntity : class
    {
        private static CsvConfiguration CsvConfiguration =
            new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null,
                Delimiter = ";",
                Mode = CsvMode.NoEscape,
                IgnoreReferences = true
            };

        private readonly string _seedFilePath;

        public CsvDataProvider(string seedFileName)
        {
            _seedFilePath = seedFileName;
        }

        public async Task<IReadOnlyCollection<TSeedEntity>> LoadSeedData(CancellationToken cancellationToken)
        {
            var folder = Path.GetDirectoryName(Environment.ProcessPath ?? string.Empty);
            var fullPath = Path.Combine(folder ?? string.Empty, _seedFilePath);

            using var reader = new StreamReader(File.OpenRead(fullPath), Encoding.UTF8);
            using var csv = new CsvReader(reader, CsvConfiguration);

            return await csv
                .GetRecordsAsync<TSeedEntity>(cancellationToken)
                .ToListAsync(cancellationToken);
        }
    }
}
