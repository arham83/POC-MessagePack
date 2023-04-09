using System;
using System.Diagnostics;
using MessagePack;

namespace Namespace
{
    class Program
    {
        public static void Test1()
        {
            var rows = new List<string>();
            var newRow = string.Format(
                "{0},{1},{2},{3},{4}",
                "LZ4 Compression",
                "MessageSize(Bytes)",
                "Serialized Message Size(Bytes)",
                "SerializingTime(ns)",
                "DeserializingTime(ns)"
            );
            rows.Add(newRow);
            ProcessStores.Exec(ref rows, "./SampleMessages/sample1.json");
            ProcessStores.Exec(ref rows, "./SampleMessages/sample2.json");
            ProcessStores.Exec(ref rows, "./SampleMessages/sample3.json");
            ProcessStores.Exec(ref rows, "./SampleMessages/sample4.json");
            Miscellaneous.CompileReport(rows, @"./Report/MessageSizeBased - Report.csv");
        }

        public static void Test3()
        {
            var rows = new List<string>();
            var newRow = string.Format(
                "{0},{1},{2},{3},{4}",
                "LZ4 Compression",
                "MessageSize(Bytes)",
                "Serialized Message Size(Bytes)",
                "SerializingTime(ns)",
                "DeserializingTime(ns)"
            );
            rows.Add(newRow);
            CompressedMessage.Exec(ref rows, "./SampleMessages/sample1.json");
            ProcessStores.Exec(ref rows, "./SampleMessages/sample1.json");
            CompressedMessage.Exec(ref rows, "./SampleMessages/sample4.json");
            ProcessStores.Exec(ref rows, "./SampleMessages/sample4.json");
            Miscellaneous.CompileReport(rows, @"./Report/CompressedMessageSizeBased - Report.csv");
        }

        public static void Test2()
        {
            var rows = new List<string>();
            var newRow = string.Format(
                "{0},{1},{2},{3}",
                "Message Type",
                "Serialized Message Size(Bytes)",
                "SerializingTime(ns)",
                "DeserializingTime(ns)"
            );
            rows.Add(newRow);
            Message1.Exec(ref rows);
            Message2.Exec(ref rows);
            Message3.Exec(ref rows);
            Message4.Exec(ref rows);
            Message5.Exec(ref rows);
            Message6.Exec(ref rows);

            Miscellaneous.CompileReport(rows, @"./Report/MessageTypeBased - Report.csv");
        }

        static void Main()
        {
            // Warmup the MessagePack
            // Message1.Check();
            // Message2.Check();
            // Message3.Check();

            // Warmup the MessagePack
            var rows = new List<string>();
            ProcessStores.Exec(ref rows, "./SampleMessages/sample1.json");
            Message1.Exec(ref rows);

            Test1();
            Test2();
            Test3();
            ImpExpKeys.Test3();
            // Console.ReadKey();
        }
    }
}
