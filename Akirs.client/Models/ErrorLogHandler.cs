using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Akirs.client.Models
{
    public class ErrorLogHandler
    {

        private static object cvLockObject = new object();
        private static string cvsLogFile = System.Configuration.ConfigurationManager.AppSettings["LogFile"];
        private static string filePath = System.Configuration.ConfigurationManager.AppSettings["LogFilePath"];
        private static string LogSize = System.Configuration.ConfigurationManager.AppSettings["LogSize"];
        private static string GetUniqueFilePath(string filepath)
        {
            if (File.Exists(filepath))
            {
                string folder = Path.GetDirectoryName(filepath);
                string filename = Path.GetFileNameWithoutExtension(filepath);
                string extension = Path.GetExtension(filepath);
                int number = 1;

                Match regex = Regex.Match(filepath, @"(.+) \((\d+)\)\.\w+");

                if (regex.Success)
                {
                    filename = regex.Groups[1].Value;
                    number = int.Parse(regex.Groups[2].Value);
                }

                do
                {
                    number++;
                    filepath = Path.Combine(folder, string.Format("{0} ({1}){2}", filename, number, extension));
                }
                while (File.Exists(filepath));
            }

            return filepath;
        }
        public static void SaveLog(string psDetails)
        {

            FileInfo f = new FileInfo(Path.Combine(filePath, cvsLogFile));
            string new_file_name = string.Empty;
            if (File.Exists(Path.Combine(filePath, cvsLogFile)))
            {
                long s1 = f.Length;
                if (s1 > Convert.ToInt32(LogSize))
                {
                    new_file_name = GetUniqueFilePath(Path.Combine(filePath, cvsLogFile));

                    string filename = new_file_name;


                    File.Move(Path.Combine(filePath, cvsLogFile), filename);
                }
            }
            lock (cvLockObject)
            {
                File.AppendAllText(Path.Combine(filePath, cvsLogFile), DateTime.Now.ToString() + ": " + psDetails + Environment.NewLine);

            }
        }
    }
}
    

