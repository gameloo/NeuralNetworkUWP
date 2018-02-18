using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;

namespace NeuralNetworkUWP
{
    static class Log
    {
        static object locker = new object();
        static StorageFile logFile;
        public static async void write(string msg)
        {
                DateTime currtime = DateTime.Now;
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            if (logFile == null)
                logFile = await localFolder.CreateFileAsync("log.txt", CreationCollisionOption.ReplaceExisting);
            else logFile = await localFolder.GetFileAsync("log.txt");

            await FileIO.AppendTextAsync(logFile, msg);

        }
        public static void delete()
        {
            if (System.IO.File.Exists(@"log.txt"))
            {
                System.IO.File.Delete(@"log.txt");
            }
        }
        public static void rename()
        {
            if (System.IO.File.Exists(@"log.txt"))
            {
                try
                {
                    DateTime currtime = DateTime.Now;
                    string txt = String.Format("{0:yyMMddhhmmss}", currtime);
                    System.IO.File.Move(@"log.txt", @"log" + txt + ".txt");
                }
                catch { }
            }
        }
        public static string getLog()
        {
            lock (locker)
            {
                return System.IO.File.ReadAllText(@"log.txt");
            }
        }
    }
}

