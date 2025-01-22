using CsvHelper.Configuration;
using SimpleETL.Configuration.CsvHelperConfiguration.Models;

namespace SimpleETL.Configuration.CsvHelperConfiguration.Maps;

internal sealed class DataEntryPrototypeMap : ClassMap<DataEntryPrototype>
{
    public DataEntryPrototypeMap()
    {
        Map(m => m.VendorID).Name("VendorID");
        Map(m => m.PickupDateTime).Name("tpep_pickup_datetime");
        Map(m => m.DropoffDateTime).Name("tpep_dropoff_datetime");
        Map(m => m.PassengerCount).Name("passenger_count");
        Map(m => m.TripDistance).Name("trip_distance");
        Map(m => m.RatecodeID).Name("RatecodeID");
        Map(m => m.StoreAndFwdFlag).Name("store_and_fwd_flag");
        Map(m => m.PULocationID).Name("PULocationID");
        Map(m => m.DOLocationID).Name("DOLocationID");
        Map(m => m.PaymentType).Name("payment_type");
        Map(m => m.FareAmount).Name("fare_amount");
        Map(m => m.Extra).Name("extra");
        Map(m => m.MtaTax).Name("mta_tax");
        Map(m => m.TipAmount).Name("tip_amount");
        Map(m => m.TollsAmount).Name("tolls_amount");
        Map(m => m.ImprovementSurcharge).Name("improvement_surcharge");
        Map(m => m.TotalAmount).Name("total_amount");
        Map(m => m.CongestionSurcharge).Name("congestion_surcharge");
    }
}