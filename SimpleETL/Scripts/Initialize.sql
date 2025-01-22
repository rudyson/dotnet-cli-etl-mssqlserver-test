﻿CREATE DATABASE simpleetl_cab;

CREATE TABLE [simpleetl_cab].[dbo].[cab_data] (
    [ID]                    INT IDENTITY (1,1) PRIMARY KEY,
    [tpep_pickup_datetime]  DATETIME        NOT NULL,
    [tpep_dropoff_datetime] DATETIME        NOT NULL,
    [passenger_count]       INT             NOT NULL,
    [trip_distance]         DECIMAL (10, 2) NOT NULL,
    [store_and_fwd_flag]    VARCHAR (3)     NOT NULL,
    [PULocationID]          INT             NOT NULL,
    [DOLocationID]          INT             NOT NULL,
    [fare_amount]           DECIMAL (10, 2) NOT NULL,
    [tip_amount]            DECIMAL (10, 2) NOT NULL
);