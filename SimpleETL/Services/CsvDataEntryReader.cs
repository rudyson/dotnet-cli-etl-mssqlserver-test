using CsvHelper;
using CsvHelper.Configuration;
using SimpleETL.Configuration.Application;
using SimpleETL.Configuration.CsvHelperConfiguration.Extensions;
using SimpleETL.Configuration.CsvHelperConfiguration.Maps;
using SimpleETL.Configuration.CsvHelperConfiguration.Models;
using System.Globalization;

namespace SimpleETL.Services;
internal class CsvDataEntryReader : IDisposable // IEnumerable<DataEntryPrototype>
{
    private readonly CsvConfiguration _configuration;
    private readonly CsvOptions _options;

    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private readonly StreamReader _inputStreamReader;
    private readonly CsvReader _inputCsvReader;

    private readonly StreamWriter _corruptedStreamWriter;
    private readonly CsvWriter _corruptedCsvWriter;

    private readonly StreamWriter _duplicatesStreamWriter;
    private readonly CsvWriter _duplicatesCsvWriter;

    private readonly TimeZoneInfo _estTimeZoneInfo;

    /// <summary>
    /// Stores unique records keys
    /// </summary>
    private readonly HashSet<string> _uniqueRecords = new();
    /// <summary>
    /// Stores records keys whether is duplicated once
    /// </summary>
    private readonly HashSet<string> _duplicateRecords = new();

    private bool _disposed = false;

    public CsvDataEntryReader(CsvOptions? csvOptions = null, CsvConfiguration? csvConfiguration = null)
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
            // Ensure that all text-based fields are free from leading or trailing whitespace
            TrimOptions = TrimOptions.Trim,
            // Ignoring parsing exception to receive null value and process it
            ReadingExceptionOccurred = _ => false
        };

        _estTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        var inputFilePath = Path.Combine(_options.WorkingDirectory, _options.InputDataFilename);
        _inputStreamReader = new StreamReader(inputFilePath);
        _inputCsvReader = new CsvReader(_inputStreamReader, _configuration);
        _inputCsvReader.Context.RegisterClassMap<DataEntryPrototypeMap>();

        var corruptedFilePath = Path.Combine(_options.WorkingDirectory, _options.CorruptedRecordsFilename);
        _corruptedStreamWriter = new StreamWriter(corruptedFilePath);
        _corruptedCsvWriter = new CsvWriter(_corruptedStreamWriter, _configuration);
        _corruptedCsvWriter.WriteHeader<DataEntryPrototype>();
        _corruptedCsvWriter.NextRecord();

        var duplicatesFilePath = Path.Combine(_options.WorkingDirectory, _options.DuplicateRecordsFilename);
        _duplicatesStreamWriter = new StreamWriter(duplicatesFilePath);
        _duplicatesCsvWriter = new CsvWriter(_duplicatesStreamWriter, _configuration);
        _duplicatesCsvWriter.WriteHeader<DataEntryPrototype>();
        _duplicatesCsvWriter.NextRecord();
    }

    public IEnumerable<DataEntryPrototype> ReadCsv() => GetRecords(_inputCsvReader);

    private IEnumerable<DataEntryPrototype> GetRecords(CsvReader csvReader)
    {
        while (csvReader.Read())
        {
            string? rawRecordCsvRow = csvReader.Context?.Parser?.RawRecord;
            if (string.IsNullOrEmpty(rawRecordCsvRow))
            {
                continue;
            }

            // Checking if record corrupted
            DataEntryPrototype? record = csvReader.GetRecord<DataEntryPrototype>();
            if (record == null)
            {
                _corruptedCsvWriter.WriteField(rawRecordCsvRow);
                _corruptedCsvWriter.NextRecord();
                continue;
            }

            // Checking if record duplicated
            var recordKey = record.GenerateKey();
            if (!_uniqueRecords.Add(recordKey))
            {
                // Writing that duplicated only first time
                if (_duplicateRecords.Add(recordKey))
                {
                    _duplicatesCsvWriter.WriteRecord(record);
                    _duplicatesCsvWriter.NextRecord();
                }

                continue;
            }

            // Normalization of entries according to requirements
            record.NormalizeDate(_estTimeZoneInfo);
            record.NormalizeFlags();

            yield return record;
        }
    }

    ~CsvDataEntryReader()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Cleans resources
    /// </summary>
    /// <param name="disposing">Specifies if Dispose method called manually</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        _cancellationTokenSource.Cancel();

        _inputCsvReader.Dispose();
        _inputStreamReader.Dispose();

        _corruptedCsvWriter.Dispose();
        _corruptedStreamWriter.Dispose();

        _duplicatesCsvWriter.Dispose();
        _duplicatesStreamWriter.Dispose();

        _disposed = true;
    }
}
