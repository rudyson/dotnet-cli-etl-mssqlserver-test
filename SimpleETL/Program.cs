using Microsoft.Extensions.Configuration;

namespace SimpleETL;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var config = BuildConfiguration();
        await SimpleEtlSolution.StartExecutionAsync(config);
    }

    private static IConfiguration BuildConfiguration()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets<SimpleEtlSolution>();
        IConfiguration configuration = builder.Build();

        return configuration;
    }
}