using System;
using System.Diagnostics;
using MessagePack;

namespace Namespace
{
    [MessagePackObject]
    public class MyObject3
    {
        [Key(0)]
        public string? MessageType { get; set; }

        [Key(1)]
        public int OuterInt { get; set; }

        [Key(2)]
        public NestedObject? NesObj { get; set; }
    }

    [MessagePackObject]
    public class NestedObject
    {
        [Key(0)]
        public int IntValue { get; set; }

        [Key(1)]
        public string? StringValue { get; set; }
    }

    public static class Message3
    {
        public static void Exec(ref List<string> rows)
        {
            // Create an instance of the object to serialize and deserialize
            var Obj = new MyObject3
            {
                MessageType = "Message having Nested Object",
                NesObj = new NestedObject() { IntValue = 99, StringValue = "sub" }
            };

            Stopwatch mpTimer1 = new Stopwatch();
            Stopwatch mpTimer2 = new Stopwatch();
            long totalTimeToSer = 0;
            long totalTimeToDeSer = 0;
            int byteSize = 0;
            var ItrationNo = 10;
            foreach (var i in Enumerable.Range(0, ItrationNo))
            {
                mpTimer1.Start();
                byte[] mpBytes = MessagePackSerializer.Serialize(Obj);
                mpTimer1.Stop();
                byteSize = mpBytes.Length;
                totalTimeToSer += mpTimer1.ElapsedTicks * (1000000000L / Stopwatch.Frequency);
                mpTimer2.Start();
                MyObject3 mpDeserialized = MessagePackSerializer.Deserialize<MyObject3>(mpBytes);
                mpTimer2.Stop();
                totalTimeToDeSer += mpTimer2.ElapsedTicks * (1000000000L / Stopwatch.Frequency);
            }
            var newRow = string.Format(
                "{0},{1},{2},{3}",
                Obj.MessageType,
                byteSize,
                totalTimeToSer / ItrationNo,
                totalTimeToDeSer / ItrationNo
            );
            rows.Add(newRow);
        }
    }
}
