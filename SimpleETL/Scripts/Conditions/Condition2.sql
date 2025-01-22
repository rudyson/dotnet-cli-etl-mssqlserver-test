--- Find the top 100 longest fares in terms of trip_distance.

SELECT TOP 100
    ID,
    PULocationID,
    DOLocationID,
    trip_distance,
    fare_amount,
    tip_amount
FROM
    [simpleetl_cab].[dbo].[cab_data]
ORDER BY
    trip_distance DESC;