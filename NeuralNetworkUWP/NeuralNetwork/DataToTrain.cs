using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace NeuralNetworkUWP.NeuralNetwork
{
    public class DataToTrain
    {
        public List<TrainSet> TrainSet { get; private set; }
        public int SizeIn { get; private set; }
        public int SizeOut { get; private set; }

        public DataToTrain(StorageFile file)
        {
            TrainSet = new List<TrainSet>();

            List<string[]> listLine = TextFieldParser.ParseFile(file, ':').Result;

            foreach(string[] halfLine in listLine)
            {

                string[] tempInputString = halfLine[0].Split(';');
                SizeIn = tempInputString.Length - 1;
                double[] tempInputSignal = new double[SizeIn];
                for (int i = 0; i < SizeIn; i++) tempInputSignal[i] = Convert.ToDouble(tempInputString[i]); // пофиксить

                string[] tempResponseString = halfLine[1].Split(';');
                SizeOut = tempResponseString.Length - 1;
                double[] tempResponse = new double[SizeOut];
                for (int i = 1; i < tempResponseString.Length; i++) tempResponse[i - 1] = Convert.ToDouble(tempResponseString[i]); // пофиксить

                TrainSet.Add(new TrainSet { InputSignal = tempInputSignal, ExpectedResponse = tempResponse });
            }
        }
    }
    public static class TextFieldParser
    {
        private const char DEFAULT_DELIMITER = ';';

        public static async Task<List<string[]>> ParseFile(StorageFile file, params char[] delimeter)
        {
            List<string[]> returnParseList = new List<string[]>();
            var readFile = await Windows.Storage.FileIO.ReadLinesAsync(file);
            foreach (var line in readFile)
            {
                if (delimeter != null)
                    returnParseList.Add(line.Split(delimeter));
                else returnParseList.Add(line.Split(DEFAULT_DELIMITER));
            }
            return returnParseList;
        }
    }
}
