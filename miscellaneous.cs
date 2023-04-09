using Newtonsoft.Json;
using System.Text;


namespace Namespace
{
    public static class Miscellaneous
    {
        public static T DeserializeJson<T>(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static long GetFileSize(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found", filePath);
            }

            FileInfo fileInfo = new FileInfo(filePath);

            return fileInfo.Length;
        }

        public static void CompileReport(List<string> rows, string path)
        {
            var csv = new StringBuilder();
            foreach (var row in rows)
            {
                csv.AppendLine(row);
            }
            File.WriteAllText(path, csv.ToString());
        }
    }
}
