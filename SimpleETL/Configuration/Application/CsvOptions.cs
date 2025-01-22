namespace SimpleETL.Configuration.Application;
internal class CsvOptions
{
    public const string SectionName = "Csv";

    public required string WorkingDirectory { get; set; }
    public required string InputDataFilename { get; set; }
    public required string CorruptedRecordsFilename { get; set; }
    public required string DuplicateRecordsFilename { get; set; }
}
