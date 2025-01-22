# .NET CLI ETL Test Application

This applications is the showcase of Extract, Transform, Load (ETL) in .NET. For extract uses csv file, then entry transforms and loads into database.

## Current code results

Results for input file with length `30003 rows` including the header:

```
[CsvDataEntryReader::Elapsed] 77,67 ms
[DatabaseService::RowsInserted] 29840
[DatabaseService::Elapsed] 1085,9579 ms
[Total::Elapsed] 1166,4381 ms
```

Output files:

- `duplicate_records.csv`, 111 rows
- `invalid_records.csv`, 51 rows

Future potential input data size: `50GB`

## Potential improvements

- CsvDataEntryReader should implement `IAsyncEnumerable` to improve performance, batch processing and task cancellation.
- DatabaseService should use ORM to avoid hardcoding of properties. Also EntityFramework allow use EnsureCreated, EnsureDeleted and Migrate methods.
- Additional API for 4 conditions. It will be allow to receive requested data. Maybe special Razor page with dashboard.
- Data table partitioning if there is a lots of data.

## Warning

> Note that batch processing is impossible whether we have to write into `duplicates.csv`. We can just insert distinct by own key and ignore repeated data to avoid loading whole duplicate keys into memory and `StackOverflowException`.

## Configuration

1. Specify output database Microsoft SQL Server connection string.
2. Specify working directory and valid file names.
3. Build solution and enjoy.

```json
{
    "ConnectionStrings": {
        "OutputDatabase": "Data Source=.\\SQLEXPRESS;Initial Catalog=simpleetl_cab;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"
    },
    "Csv": {
        "WorkingDirectory": "./",
        "InputDataFilename": "input.csv",
        "CorruptedRecordsFilename": "corrupted.csv",
        "DuplicateRecordsFilename": "duplicates.csv"
    }
}
```

## Data types comparation

| SQL-Stored | Column Name           | Data Type (CSV) | Data Example           | Data Type (C#) | Data Type (MSSQL Server / TSQL) | Cast conventions  |
| ---------- | --------------------- | --------------- | ---------------------- | -------------- | ------------------------------- | ----------------- |
|            | VendorID              | Integer         | 1                      | `int`          | `INT`                           |                   |
| [+]        | tpep_pickup_datetime  | DateTime        | 01/01/2020 12:28:15 AM | `DateTime`     | `DATETIME`                      | EST -> UTC        |
| [+]        | tpep_dropoff_datetime | DateTime        | 01/01/2020 12:33:03 AM | `DateTime`     | `DATETIME`                      | EST -> UTC        |
| [+]        | passenger_count       | Integer         | 1                      | `int`          | `INT`                           |                   |
| [+]        | trip_distance         | Decimal         | 1.2                    | `decimal`      | `DECIMAL(10, 2)`                |                   |
|            | RatecodeID            | Integer         | 1                      | `int`          | `INT`                           |                   |
| [+]        | store_and_fwd_flag    | String          | N                      | `string`       | `VARCHAR(3)`                    | N -> No, Y -> Yes |
| [+]        | PULocationID          | Integer         | 238                    | `int`          | `INT`                           |                   |
| [+]        | DOLocationID          | Integer         | 239                    | `int`          | `INT`                           |                   |
|            | payment_type          | Integer         | 1                      | `int`          | `INT`                           |                   |
| [+]        | fare_amount           | Decimal         | 6                      | `decimal`      | `DECIMAL(10, 2)`                |                   |
|            | extra                 | Decimal         | 3                      | `decimal`      | `DECIMAL(10, 2)`                |                   |
|            | mta_tax               | Decimal         | 0.5                    | `decimal`      | `DECIMAL(10, 2)`                |                   |
| [+]        | tip_amount            | Decimal         | 1.47                   | `decimal`      | `DECIMAL(10, 2)`                |                   |
|            | tolls_amount          | Decimal         | 0                      | `decimal`      | `DECIMAL(10, 2)`                |                   |
|            | improvement_surcharge | Decimal         | 0.3                    | `decimal`      | `DECIMAL(10, 2)`                |                   |
|            | total_amount          | Decimal         | 11.27                  | `decimal`      | `DECIMAL(10, 2)`                |                   |
|            | congestion_surcharge  | Decimal         | 2.5                    | `decimal`      | `DECIMAL(10, 2)`                |                   |
