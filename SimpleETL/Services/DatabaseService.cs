using Microsoft.Data.SqlClient;
using SimpleETL.Configuration.CsvHelperConfiguration.Models;
using System.Data;
using System.Globalization;

namespace SimpleETL.Services;
internal class DatabaseService
{
    private readonly string _connectionString;
    private const string tableName = "cab_data";
    public DatabaseService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public bool IsServerConnected()
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        try
        {
            sqlConnection.Open();
            return true;
        }
        catch (SqlException)
        {
            return false;
        }
    }
    // public Task WriteToServerAsync(DataTable table, CancellationToken cancellationToken)
    public int InsertData(IEnumerable<DataEntryPrototype> data)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        sqlConnection.Open();

        using var bulkCopy = new SqlBulkCopy(sqlConnection)
        {
            DestinationTableName = $"[{sqlConnection.Database}].[dbo].[{tableName}]"
        };
        SetupColumnMappings(bulkCopy.ColumnMappings);

        var dataTable = new DataTable
        {
            Locale = CultureInfo.InvariantCulture
        };
        SetupDataColumnCollection(dataTable.Columns);

        foreach (var entry in data)
        {
            InsertRow(dataTable.Rows, entry);
        }

        try
        {
            bulkCopy.WriteToServer(dataTable);
        }
        catch (InvalidOperationException exception)
        {
            return -1;
        }

        return dataTable.Rows.Count;
    }

    private static void SetupColumnMappings(SqlBulkCopyColumnMappingCollection sqlBulkCopyColumnMappingCollection)
    {
        sqlBulkCopyColumnMappingCollection.Add("tpep_pickup_datetime", "tpep_pickup_datetime");
        sqlBulkCopyColumnMappingCollection.Add("tpep_dropoff_datetime", "tpep_dropoff_datetime");
        sqlBulkCopyColumnMappingCollection.Add("passenger_count", "passenger_count");
        sqlBulkCopyColumnMappingCollection.Add("trip_distance", "trip_distance");
        sqlBulkCopyColumnMappingCollection.Add("store_and_fwd_flag", "store_and_fwd_flag");
        sqlBulkCopyColumnMappingCollection.Add("PULocationID", "PULocationID");
        sqlBulkCopyColumnMappingCollection.Add("DOLocationID", "DOLocationID");
        sqlBulkCopyColumnMappingCollection.Add("fare_amount", "fare_amount");
        sqlBulkCopyColumnMappingCollection.Add("tip_amount", "tip_amount");
    }

    private static void SetupDataColumnCollection(DataColumnCollection dataColumnCollection)
    {
        dataColumnCollection.Add("tpep_pickup_datetime", typeof(DateTime));
        dataColumnCollection.Add("tpep_dropoff_datetime", typeof(DateTime));
        dataColumnCollection.Add("passenger_count", typeof(int));
        dataColumnCollection.Add("trip_distance", typeof(decimal));
        dataColumnCollection.Add("store_and_fwd_flag", typeof(string));
        dataColumnCollection.Add("PULocationID", typeof(int));
        dataColumnCollection.Add("DOLocationID", typeof(int));
        dataColumnCollection.Add("fare_amount", typeof(decimal));
        dataColumnCollection.Add("tip_amount", typeof(decimal));
    }

    private static void InsertRow(DataRowCollection dataRowCollection, DataEntryPrototype entry)
    {
        dataRowCollection.Add(
                entry.PickupDateTime,
                entry.DropoffDateTime,
                entry.PassengerCount,
                entry.TripDistance,
                entry.StoreAndFwdFlag,
                entry.PULocationID,
                entry.DOLocationID,
                entry.FareAmount,
                entry.TipAmount
            );
    }
}
