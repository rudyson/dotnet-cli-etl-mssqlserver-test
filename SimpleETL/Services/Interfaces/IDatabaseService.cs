namespace SimpleETL.Services.Interfaces;

public interface IDatabaseService<T> where T : class
{
    Task<bool> IsServerConnectedAsync(CancellationToken cancellationToken = default);
    Task<int> InsertDataAsync(IEnumerable<T> data, CancellationToken cancellationToken = default);
}
