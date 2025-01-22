DROP INDEX IDX_PULocationID ON [simpleetl_cab].[dbo].[cab_data];
DROP INDEX IDX_TipAmount ON [simpleetl_cab].[dbo].[cab_data];
DROP INDEX IDX_TripDistance ON [simpleetl_cab].[dbo].[cab_data];
DROP INDEX IDX_PickupDateTime ON [simpleetl_cab].[dbo].[cab_data];
DROP INDEX IDX_DropoffDateTime ON [simpleetl_cab].[dbo].[cab_data];

DROP TABLE [simpleetl_cab].[dbo].[cab_data];

DROP DATABASE simpleetl_cab;