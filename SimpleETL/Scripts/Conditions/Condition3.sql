--- Find the top 100 longest fares in terms of time spent traveling.

SELECT TOP 100
    ID,
    PULocationID,
    DOLocationID,
    DATEDIFF(SECOND, tpep_pickup_datetime, tpep_dropoff_datetime) AS trip_duration_seconds,
    fare_amount,
    tip_amount
FROM
    [simpleetl_cab].[dbo].[cab_data]
ORDER BY
    trip_duration_seconds DESC;