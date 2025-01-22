using CsvHelper;
using CsvHelper.Configuration;
using SimpleETL.Configuration.Application;
using SimpleETL.Configuration.CsvHelperConfiguration.Extensions;
using SimpleETL.Configuration.CsvHelperConfiguration.Maps;
using SimpleETL.Configuration.CsvHelperConfiguration.Models;
using System.Globalization;

namespace SimpleETL.Services;
internal class CsvReaderService : IDisposable
{
    private readonly CsvConfiguration _configuration;
    private readonly CsvOptions _options;

    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly StreamReader _streamReader;
    private readonly CsvReader _csvReader;

    private readonly HashSet<string> _uniqueRecords = new();
    private readonly HashSet<string> _invalidRecords = new();
    private readonly HashSet<string> _duplicateRecords = new();
    private bool _disposed = false;

    public CsvReaderService(CsvOptions? csvOptions = null, CsvConfiguration? csvConfiguration = null)
    {
        _options = csvOptions ?? new CsvOptions
        {
            WorkingDirectory = string.Empty,
            CorruptedRecordsFilename = "corrupted.csv",
            DuplicateRecordsFilename = "duplicates.csv",
            InputDataFilename = "input.csv"
        };

        _configuration = csvConfiguration ?? new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            // Trimming string values
            TrimOptions = TrimOptions.Trim,
            // Ignoring parsing exception to receive null value
            ReadingExceptionOccurred = _ => false
        };

        var filePath = _options.WorkingDirectory + _options.InputDataFilename;

        _streamReader = new StreamReader(filePath);
        _csvReader = new CsvReader(_streamReader, _configuration);
        _csvReader.Context.RegisterClassMap<DataEntryPrototypeMap>();
    }

    public IEnumerable<DataEntryPrototype> ReadCsv()
    {
        var result = GetRecords(_csvReader);

        //Console.WriteLine($"Duplicate records: {_duplicateRecords.Count}");
        //Console.WriteLine($"Corrupted records: {_invalidRecords.Count}");

        return result;
    }

    private IEnumerable<DataEntryPrototype> GetRecords(CsvReader csvReader)
    {
        TimeZoneInfo estTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        while (csvReader.Read())
        {
            string? rawRecordCsvRow = csvReader.Context?.Parser?.RawRecord;
            if (string.IsNullOrEmpty(rawRecordCsvRow))
            {
                continue;
            }

            DataEntryPrototype? record = csvReader.GetRecord<DataEntryPrototype>();
            if (record == null)
            {
                _invalidRecords.Add(rawRecordCsvRow);
                continue;
            }

            var recordKey = record.GenerateKey();

            if (!_uniqueRecords.Add(recordKey))
            {
                _duplicateRecords.Add(rawRecordCsvRow);
                continue;
            }

            record.NormalizeDate(estTimeZoneInfo);
            record.NormalizeFlags();

            yield return record;
        }
    }

    ~CsvReaderService()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        _cancellationTokenSource.Cancel();
        _csvReader.Dispose();
        _streamReader.Dispose();

        _disposed = true;
    }
}
