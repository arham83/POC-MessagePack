using System;
using System.Diagnostics;
using MessagePack;

namespace Namespace
{
    public static class CompressedMessage
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
                var lz4Options = MessagePackSerializerOptions.Standard.WithCompression(
                    MessagePackCompression.Lz4BlockArray
                );
                stopwatch1.Start();
                byte[] messagePackBytes = MessagePackSerializer.Serialize(storeInfoIK,lz4Options);
                stopwatch1.Stop();
                byteSize = messagePackBytes.Length;
                totalTimeToSer += stopwatch1.ElapsedTicks * (1000000000L / Stopwatch.Frequency);
                stopwatch2.Start();
                var SIK = MessagePackSerializer.Deserialize<StoreData>(messagePackBytes,lz4Options);
                stopwatch2.Stop();
                totalTimeToDeSer += stopwatch2.ElapsedTicks * (1000000000L / Stopwatch.Frequency);
            }
            var newRow = string.Format(
                "{0},{1},{2},{3},{4}",
                "Enabled",
                Miscellaneous.GetFileSize(path),
                byteSize,
                totalTimeToSer / ItrationNo,
                totalTimeToDeSer / ItrationNo
            );
            rows.Add(newRow);
        }
    }
}
