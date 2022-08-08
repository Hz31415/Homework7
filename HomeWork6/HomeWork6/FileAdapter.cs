using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Globalization;

namespace HomeWork6
{
    internal static class FileAdapter
    {
        public static void Create()
        {
            if (!File.Exists(@"Workers.txt"))
            {
                File.Create(@"Workers.txt");
            }
        }

        public static async Task<string> Read()
        {
            string fileData;
            using (FileStream fstream = File.OpenRead(@"Workers.txt"))
            {
                byte[] buffer = new byte[fstream.Length];
                await fstream.ReadAsync(buffer, 0, buffer.Length);
                fileData = Encoding.Default.GetString(buffer);
            }
            return fileData;
        }

        public static async Task Write(string fileData)
        {
            using (FileStream fstream = new FileStream(@"Workers.txt", FileMode.OpenOrCreate))
            {
                fstream.Seek(0, SeekOrigin.End);
                byte[] buffer = Encoding.Default.GetBytes(fileData);
                await fstream.WriteAsync(buffer, 0, buffer.Length);
            }
        }
    }
}
