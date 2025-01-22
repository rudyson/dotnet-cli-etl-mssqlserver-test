-- Search, where part of the conditions is PULocationId

DECLARE @PULocationID INT;
SET @PULocationID = 238;

SELECT
    ID,
    PULocationID,
    DOLocationID,
    tpep_pickup_datetime,
    tpep_dropoff_datetime,
    trip_distance,
    fare_amount,
    tip_amount
FROM
    [cab_data]
WHERE
    PULocationID = @PULocationID
ORDER BY
    tpep_pickup_datetime DESC;