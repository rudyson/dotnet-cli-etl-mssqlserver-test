# .NET CLI ETL Test Application

Input data: `sample-cad-data.csv`

## Data types comparation

| SQL-Stored | Column Name             | Data Type (CSV)        | Data Example                 | Data Type (C#)       | Data Type (MSSQL Server / TSQL) | Cast conventions |
|--------------------|-------------------------|------------------------|-----------------------------|----------------------|---------------------------------|-----|
|  | VendorID               | Integer               | 1                           | `int`                | `INT`                           |    |
| [+] | tpep_pickup_datetime   | DateTime              | 01/01/2020 12:28:15 AM      | `DateTime`           | `DATETIME`                     | EST -> UTC   |
|[+] | tpep_dropoff_datetime  | DateTime              | 01/01/2020 12:33:03 AM      | `DateTime`           | `DATETIME`                     | EST -> UTC   |
| [+]| passenger_count        | Integer               | 1                           | `int`                | `INT`                           |    |
|[+] | trip_distance          | Decimal               | 1.2                         | `decimal`            | `DECIMAL(10, 2)`                |    |
| | RatecodeID             | Integer               | 1                           | `int`                | `INT`                           |    |
|[+] | store_and_fwd_flag     | String                | N                           | `string`             | `VARCHAR(3)`                       | N -> No, Y -> Yes   |
|[+] | PULocationID           | Integer               | 238                         | `int`                | `INT`                           |    |
|[+] | DOLocationID           | Integer               | 239                         | `int`                | `INT`                           |    |
| | payment_type           | Integer               | 1                           | `int`                | `INT`                           |    |
|[+] | fare_amount            | Decimal               | 6                           | `decimal`            | `DECIMAL(10, 2)`                |    |
| | extra                  | Decimal               | 3                           | `decimal`            | `DECIMAL(10, 2)`                |    |
| | mta_tax                | Decimal               | 0.5                         | `decimal`            | `DECIMAL(10, 2)`                |    |
|[+] | tip_amount             | Decimal               | 1.47                        | `decimal`            | `DECIMAL(10, 2)`                |    |
| | tolls_amount           | Decimal               | 0                           | `decimal`            | `DECIMAL(10, 2)`                |    |
| | improvement_surcharge  | Decimal               | 0.3                         | `decimal`            | `DECIMAL(10, 2)`                |    |
| | total_amount           | Decimal               | 11.27                       | `decimal`            | `DECIMAL(10, 2)`                |    |
| | congestion_surcharge   | Decimal               | 2.5                         | `decimal`            | `DECIMAL(10, 2)`                |    |

---