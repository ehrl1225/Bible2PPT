using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace Bible2PPT.Services.BibleService.Sources
{
    internal class FileManager
    {
        public static string[] LoadFileNames()
        {
            string path = "bible/text";
            var result = Directory.GetFiles(path).Select(Path.GetFileName).ToArray();
            return result;
        }

        public static string remove_number(string bookName)
        {
            if (bookName.Length < 4)
            {
                return bookName;
            }
            return bookName.Substring(4);
        }

        public static string remove_extension(string bookName)
        {
            if (string.IsNullOrEmpty(bookName))
            {
                return bookName;
            }
            int lastDot = bookName.LastIndexOf('.');
            if (lastDot == -1) {
                return bookName;
            }
            return bookName.Substring(0, lastDot);
        }

        public static string LoadFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }
            return File.ReadAllText(filePath, Encoding.GetEncoding("EUC-KR"));
        }

        public static List<int> getIndexs(string bookName)
        {
            string index_path = Path.Combine("bible", "index", bookName + ".json");
            if (!File.Exists(index_path))
            {
                throw new FileNotFoundException();
            }
            string jsonString = File.ReadAllText(index_path, Encoding.GetEncoding("UTF-8"));
            
            List<int> indexes = JsonSerializer.Deserialize<List<int>>(jsonString);
            return indexes;
        }

        public static async Task<string> LoadFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }
            return await File.ReadAllTextAsync(filePath, Encoding.GetEncoding("EUC-KR"));
        }

        public static string ReadLine(string filePath, int line)
        {
            using (var reader = new StreamReader(filePath))
            {
                for (var i = 1; i < line; i++)
                {
                    if (reader.ReadLine() == null)
                    {
                        throw new Exception("해당 줄이 없습니다.");
                    }
                }
                return reader.ReadLine() ?? throw new Exception("해당 줄이 없습니다.");
            }
        }

        public static List<string> ReadFromToLine(string filePath, int startLine, int endLine)
        {
            var result = new List<string>();
            var lines = File.ReadLines(filePath, Encoding.GetEncoding("EUC-KR"));

            if (endLine > 0)
            {
                lines = lines.Take(endLine);
            }

            result.AddRange(lines.Skip(startLine - 1));

            return result;
        }
    }
}
