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
        static StorageFile logFile;
        public static async void write(string msg)
        {
            DateTime currtime = DateTime.Now;
            try
            {
                StorageFolder localFolder = await DownloadsFolder.CreateFolderAsync("Log");
            if (logFile == null)
                logFile = await localFolder.CreateFileAsync("log.txt", CreationCollisionOption.ReplaceExisting);
            else logFile = await localFolder.GetFileAsync("log.txt");
            string tempTxt = String.Format("{0:yyMMdd hh:mm:ss}\n{1}", currtime, msg);
            await FileIO.AppendTextAsync(logFile, tempTxt);
            }
            catch 
            {

            }

        }
    }
}

