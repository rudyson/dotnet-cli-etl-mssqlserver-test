namespace SimpleETL.Configuration.CsvHelperConfiguration.Models;

internal class DataEntryPrototype
{
    public int VendorID { get; set; }
    public DateTime PickupDateTime { get; set; }
    public DateTime DropoffDateTime { get; set; }
    public int PassengerCount { get; set; }
    public decimal TripDistance { get; set; }
    public int RatecodeID { get; set; }
    public required string StoreAndFwdFlag { get; set; }
    public int PULocationID { get; set; }
    public int DOLocationID { get; set; }
    public int PaymentType { get; set; }
    public decimal FareAmount { get; set; }
    public decimal Extra { get; set; }
    public decimal MtaTax { get; set; }
    public decimal TipAmount { get; set; }
    public decimal TollsAmount { get; set; }
    public decimal ImprovementSurcharge { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal CongestionSurcharge { get; set; }
}
