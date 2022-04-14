using System.Globalization;
using CodeExercise.Model;
using CsvHelper;
using Microsoft.Extensions.Logging;

namespace CodeExercise.LocationRepository;

public class CsvLocationDataLoader : ILocationDataProvider
{
    private readonly ILogger<CsvLocationDataLoader> _logger;
    private readonly string _sourceFile;

    /// <summary>
    /// Create instance of the CsvLocationDataLoader
    /// </summary>
    /// <param name="logger">Default logger</param>
    /// <param name="sourceFile">Path to the source csv</param>
    public CsvLocationDataLoader(ILogger<CsvLocationDataLoader> logger, string sourceFile = "locations.csv")
    {
        _logger = logger;
        _sourceFile = sourceFile;
    }

    public IEnumerable<ILocation> GetData()
    {
        try
        {
            using var sr = new StreamReader(_sourceFile);
            using var csv = new CsvReader(sr, CultureInfo.InvariantCulture);
            var locations = csv.GetRecords<Location>().ToList<ILocation>();
            return locations;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to load locations : {Error}", e.Message);
            // rethrow as this is a critical failure
            throw;
        }
    }
}