using System;
using System.Diagnostics;
using MessagePack;

// It's not possible to declare a pointer to a nested class in C#. In C#, objects are reference types,
// which means that they are always accessed through a reference (i.e., a pointer to the object). However,
// you cannot declare a pointer to a specific instance of a class or a nested class.
namespace Namespace
{
    [MessagePackObject]
    public class MyObject6
    {
        [Key(0)]
        public string? MessageType { get; set; }

        [Key(1)]
        public MyStruct? MyStruct { get; set; }
    }

    [MessagePackObject]
    public struct MyStruct
    {
        [Key(0)]
        public int StructInt { get; set; }

        [Key(1)]
        public string StructString { get; set; }
    }

    public static class Message6
    {
        public static void Exec(ref List<string> rows)
        {
            // Create an instance of the object to serialize and deserialize
            var Obj = new MyObject6
            {
                MessageType = "Message having nested structure",
                MyStruct = new MyStruct
                {
                    StructInt = 123,
                    StructString = "Hello, World!"
                }
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
                MyObject6 mpDeserialized = MessagePackSerializer.Deserialize<MyObject6>(mpBytes);
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
