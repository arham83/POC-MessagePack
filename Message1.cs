using System;
using System.Diagnostics;
using MessagePack;

namespace Namespace
{
    [MessagePackObject]
    public class MyObject
    {
        [Key(0)]
        public string? MessageType { get; set; }

        [Key(1)]
        public string? Property1 { get; set; }

        [Key(2)]
        public int Property2 { get; set; }
    }

    public static class Message1
    {
        public static void Exec(ref List<string> rows)
        {
            // Create an instance of the object to serialize and deserialize
            var Obj = new MyObject
            {
                MessageType = "Simple Message having two ppts",
                Property1 = "hello",
                Property2 = 42
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
                MyObject mpDeserialized = MessagePackSerializer.Deserialize<MyObject>(mpBytes);
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
