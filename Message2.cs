using System;
using System.Diagnostics;
using MessagePack;

namespace Namespace
{
    [MessagePackObject]
    public class MyObject1
    {
        [Key(0)]
        public string? MessageType { get; set; }

        [Key(1)]
        public int? IntValue { get; set; }

        [Key(2)]
        public string? StringValue { get; set; }

        [Key(3)]
        public bool BoolValue { get; set; }

        [Key(4)]
        public float FloatValue { get; set; }

        [Key(5)]
        public int[]? IntArrayValue { get; set; }
    }

    public static class Message2
    {
        public static void Exec(ref List<string> rows)
        {
            // Create an instance of the object to serialize and deserialize
            var Obj = new MyObject1
            {
                MessageType = "Simple Message having four ppts and an array",
                IntValue = 99,
                StringValue = "first",
                BoolValue = true,
                FloatValue = 3.14f,
                IntArrayValue = new int[] { 1, 2, 3 }
            };

            // MessagePack
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
                MyObject1 mpDeserialized = MessagePackSerializer.Deserialize<MyObject1>(mpBytes);
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
