using SimpleETL.Configuration.CsvHelperConfiguration.Models;

namespace SimpleETL.Configuration.CsvHelperConfiguration.Extensions;
internal static class DataEntryPrototypeExtensions
{
    public static string GenerateKey(this DataEntryPrototype record)
    {
        return $"{record.PickupDateTime:O}|{record.DropoffDateTime:O}|{record.PassengerCount}";
    }

    public static void NormalizeDate(this DataEntryPrototype record, TimeZoneInfo estTimeZoneInfo)
    {
        record.DropoffDateTime = TimeZoneInfo.ConvertTimeToUtc(record.DropoffDateTime, estTimeZoneInfo);
        record.PickupDateTime = TimeZoneInfo.ConvertTimeToUtc(record.PickupDateTime, estTimeZoneInfo);
    }

    public static void NormalizeFlags(this DataEntryPrototype record)
    {
        if (record.StoreAndFwdFlag == "Y")
        {
            record.StoreAndFwdFlag = "Yes";
        }
        else if (record.StoreAndFwdFlag == "N")
        {
            record.StoreAndFwdFlag = "No";
        }
    }
}
