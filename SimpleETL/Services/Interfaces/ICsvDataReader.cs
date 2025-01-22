namespace SimpleETL.Services.Interfaces;

interface ICsvDataReader<out T> where T : class
{
    IEnumerable<T> ReadCsv();
}
