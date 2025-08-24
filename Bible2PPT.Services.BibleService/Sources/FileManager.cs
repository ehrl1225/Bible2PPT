using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Bible2PPT.Services.BibleService.Sources
{
    class BibleFile
    {
        private string name;
        private int[] pageIndexs;

        public BibleFile(
            string name,
            int[] pageIndexs
            )
        {
            this.name = name;
            this.pageIndexs = pageIndexs;
        }

        public String getName => this.name;
        public int[] getPageIndexs() => this.pageIndexs;
    }
    internal class FileManager
    {
        public FileManager()
        {

        }

        public string LoadFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }
            return File.ReadAllText(filePath, Encoding.GetEncoding("EUC-KR"));
        }

        public async Task<string> LoadFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }
            return await File.ReadAllTextAsync(filePath, Encoding.GetEncoding("EUC-KR"));
        }
    }
}
