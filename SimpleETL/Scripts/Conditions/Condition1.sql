--- Find out which PULocationID has the highest tip_amount on average.

SELECT TOP 1
    PULocationID,
    AVG(tip_amount) AS avg_tip_amount
FROM
    [simpleetl_cab].[dbo].[cab_data]
GROUP BY
    PULocationID
ORDER BY
    avg_tip_amount DESC;