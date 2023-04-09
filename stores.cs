using System;
using System.Diagnostics;
using System.Text;
using MessagePack;

namespace Namespace
{
    [MessagePackObject]
    public class StoreData
    {
        [Key(0)]
        public Stores? Stores { get; set; }
    }

    [MessagePackObject]
    public class Stores
    {
        [Key(0)]
        public List<Store>? Store { get; set; }
    }

    [MessagePackObject]
    public class Store
    {
        [Key(0)]
        public string? StoreId { get; set; }

        [Key(1)]
        public string? Name { get; set; }

        [Key(2)]
        public string? Address1 { get; set; }

        [Key(3)]
        public string? City { get; set; }

        [Key(4)]
        public string? PostalCode { get; set; }

        [Key(5)]
        public string? StateCode { get; set; }

        [Key(6)]
        public string? CountryCode { get; set; }

        [Key(7)]
        public string? Phone { get; set; }

        [Key(8)]
        public StoreHours? StoreHours { get; set; }

        [Key(9)]
        public string? Latitude { get; set; }

        [Key(10)]
        public string? Longitude { get; set; }

        [Key(11)]
        public string? StoreLocatorEnabledFlag { get; set; }

        [Key(12)]
        public string? DemandwarePosEnabledFlag { get; set; }

        [Key(13)]
        public string? PosEnabledFlag { get; set; }

        [Key(14)]
        public CustomAttributes? CustomAttributes { get; set; }
    }

    [MessagePackObject]
    public class StoreHours
    {
        [Key(0)]
        public string? Lang { get; set; }

        [Key(1)]
        public string? Text { get; set; }
    }

    [MessagePackObject]
    public class CustomAttributes
    {
        [Key(0)]
        public List<CustomAttribute>? CustomAttribute { get; set; }
    }

    [MessagePackObject]
    public class CustomAttribute
    {
        [Key(0)]
        public string? AttributeId { get; set; }

        [Key(1)]
        public string? Text { get; set; }
    }

    public static class ProcessStores
    {
        public static void Exec(ref List<string> rows, string path)
        {
            Stopwatch stopwatch1 = new();
            Stopwatch stopwatch2 = new();
            long totalTimeToSer = 0;
            long totalTimeToDeSer = 0;
            int byteSize = 0;
            var ItrationNo = 10;
            foreach (var i in Enumerable.Range(0, ItrationNo))
            {
                StoreData storeInfoIK = Miscellaneous.DeserializeJson<StoreData>(path);
                stopwatch1.Start();
                byte[] messagePackBytes = MessagePackSerializer.Serialize(storeInfoIK);
                stopwatch1.Stop();
                totalTimeToSer += stopwatch1.ElapsedTicks * (1000000000L / Stopwatch.Frequency);
                byteSize = messagePackBytes.Length;
                stopwatch2.Start();
                var SIK = MessagePackSerializer.Deserialize<StoreData>(messagePackBytes);
                stopwatch2.Stop();
                totalTimeToDeSer += stopwatch2.ElapsedTicks * (1000000000L / Stopwatch.Frequency);
            }
            var newRow = string.Format(
                "{0},{1},{2},{3},{4}",
                "Disabled",
                Miscellaneous.GetFileSize(path),
                byteSize,
                totalTimeToSer / ItrationNo,
                totalTimeToDeSer / ItrationNo
            );
            rows.Add(newRow);
        }

    }
}
