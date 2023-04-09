using System;
using System.Diagnostics;
using System.Text;
using MessagePack;

// implementing Pub/Sub model

namespace Namespace
{
    [MessagePackObject]
    public class OuterStruct
    {
        [Key(0)]
        public int OuterInt { get; set; }

        [Key(1)]
        public InnerStruct? Inner { get; set; }

        [MessagePackObject]
        public class InnerStruct
        {
            [Key(0)]
            public int InnerInt { get; set; }

            [Key(1)]
            public string? InnerString { get; set; }
        }
    }

    [MessagePackObject(keyAsPropertyName: true)]
    public class OuterStructWithoutKeys
    {
        public int OuterInt { get; set; }

        public InnerStructWithoutKeys? Inner { get; set; }

        [MessagePackObject(keyAsPropertyName: true)]
        public class InnerStructWithoutKeys
        {
            public int InnerInt { get; set; }

            public string? InnerString { get; set; }
        }
    }

    public static class ImpExpKeys
    {
        public static void Test3()
        {
            Stopwatch stopwatch1 = new();
            Stopwatch stopwatch2 = new();
            var rows = new List<string>();
            var newRow = string.Format(
                "{0},{1},{2},{3}",
                "Serialized Message Size(Bytes)",
                "Keys",
                "SerializingTime(ns)",
                "DeserializingTime(ns)"
            );
            rows.Add(newRow);
            var myOuterStructWithoutKeys = new OuterStructWithoutKeys
            {
                OuterInt = 42,
                Inner = new OuterStructWithoutKeys.InnerStructWithoutKeys
                {
                    InnerInt = 99,
                    InnerString = "hello world"
                }
            };
            var myOuterStruct = new OuterStruct
            {
                OuterInt = 42,
                Inner = new OuterStruct.InnerStruct { InnerInt = 99, InnerString = "hello world" }
            };
            stopwatch1.Start();
            byte[] messagePackBytes = MessagePackSerializer.Serialize(myOuterStruct);
            stopwatch1.Stop();
            int byteSize = messagePackBytes.Length;
            string messagePackHex = BitConverter.ToString(messagePackBytes);
            stopwatch2.Start();
            var oS = MessagePackSerializer.Deserialize<OuterStruct>(messagePackBytes);
            stopwatch2.Stop();
            newRow = string.Format(
                "{0},{1},{2},{3}",
                byteSize,
                "Including Key Attribute",
                stopwatch1.ElapsedTicks * (1000000000L / Stopwatch.Frequency),
                stopwatch2.ElapsedTicks * (1000000000L / Stopwatch.Frequency)
            );
            rows.Add(newRow);

            // Console.Wr iteLine(oS.OuterInt);
            // Console.WriteLine(oS.Inner.InnerInt);
            // Console.WriteLine(oS.Inner.InnerString);

            stopwatch1.Start();
            byte[] messagePackBytes2 = MessagePackSerializer.Serialize(myOuterStructWithoutKeys);
            stopwatch1.Stop();
            int byteSize2 = messagePackBytes2.Length;
            string messagePackHex2 = BitConverter.ToString(messagePackBytes2);

            stopwatch2.Start();
            var oSwkeys = MessagePackSerializer.Deserialize<OuterStructWithoutKeys>(
                messagePackBytes2
            );
            stopwatch2.Stop();
            newRow = string.Format(
                "{0},{1},{2},{3}",
                byteSize2,
                "Excluding Key Attribute",
                stopwatch1.ElapsedTicks * (1000000000L / Stopwatch.Frequency),
                stopwatch2.ElapsedTicks * (1000000000L / Stopwatch.Frequency)
            );
            rows.Add(newRow);

            // Console.WriteLine(oSwkeys.OuterInt);
            // Console.WriteLine(oSwkeys.Inner.InnerInt);
            // Console.WriteLine(oSwkeys.Inner.InnerString);
            Miscellaneous.CompileReport(rows, @"./Report/IncludingOrExcludingKeys - Report.csv");
        }
    }
}
