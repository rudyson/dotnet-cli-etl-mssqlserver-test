using Microsoft.Extensions.Configuration;
using SimpleETL.Configuration.Application;
using SimpleETL.Services;
using System.Diagnostics;

namespace SimpleETL;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var config = BuildConfiguration();

        await StartExecutionAsync(config);
    }

    private static async Task StartExecutionAsync(IConfiguration configuration, CancellationToken cancellationToken = default)
    {
        var connectionString = configuration.GetConnectionString("OutputDatabase");
        var csvReaderOptions = configuration.GetSection(CsvOptions.SectionName).Get<CsvOptions>()!;

        var databaseService = new DatabaseService(connectionString!);
        if (!await databaseService.IsServerConnectedAsync(cancellationToken))
        {
            Console.WriteLine("Unable to connect to the database");
            return;
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        using var csvReader = new CsvDataEntryReader(csvReaderOptions);

        var csvDataEnumerable = csvReader.ReadCsv();
        Console.WriteLine($"[{nameof(CsvDataEntryReader)}::Elapsed] {stopwatch.Elapsed.TotalMilliseconds} ms");
        var totalElapsed = stopwatch.Elapsed.TotalMilliseconds;

        stopwatch.Reset();
        stopwatch.Start();

        var numberOfRowsInserted = await databaseService.InsertDataAsync(csvDataEnumerable, cancellationToken);

        Console.WriteLine($"[{nameof(DatabaseService)}::RowsInserted] {numberOfRowsInserted}");
        Console.WriteLine($"[{nameof(DatabaseService)}::Elapsed] {stopwatch.Elapsed.TotalMilliseconds} ms");
        totalElapsed += stopwatch.Elapsed.TotalMilliseconds;

        Console.WriteLine($"[Total::Elapsed] {totalElapsed} ms");
    }

    private static IConfiguration BuildConfiguration()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);
        IConfiguration configuration = builder.Build();

        return configuration;
    }
}