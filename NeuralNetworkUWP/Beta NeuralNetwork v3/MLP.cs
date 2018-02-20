using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NeuralNetworkUWP.Beta_NeuralNetwork_v3
{
    public class MLP
    {
        [XmlIgnore]
        private NeuralNetworkUWP.NeuralNetwork.DataToTrain dataToTrain;

        [XmlElement("InLayer")]
        public InLayer inLayer;

        [XmlElement("OutLayer")]
        public OutLayer outLayer;

        [XmlArrayItem("HiddenLayer")]
        public HiddenLayer[] hiddenLayer;

        [XmlIgnore]
        public double RequiredErrorSize { get; set; }

        [XmlAttribute]
        public int sizeIN;

        [XmlAttribute]
        public int sizeOUT;

        [XmlIgnore]
        public List<string> LogString { get; }

        [XmlIgnore]
        object locker = new object();

        public MLP() { }

        public MLP(NeuralNetworkUWP.NeuralNetwork.DataToTrain dataToTrain, double eps, double alpha, params int[] sizeHidden)
        {
            this.dataToTrain = dataToTrain;
            sizeIN = dataToTrain.SizeIn;
            sizeOUT = dataToTrain.SizeOut;

            hiddenLayer = new HiddenLayer[sizeHidden.Length];
            inLayer = new InLayer(sizeIN, hiddenLayer[0], eps, alpha);

            for (int i = 0, j = -1, q = 1; i < hiddenLayer.Length; i++, j++, q++)
            {
                if (i == 0)
                {
                    if (hiddenLayer.Length == 1)
                    {
                        hiddenLayer[i] = new HiddenLayer(sizeHidden[i], sizeIN, inLayer, outLayer, eps, alpha);
                        outLayer = new OutLayer(sizeOUT, hiddenLayer[i].Size, hiddenLayer[i], eps, alpha);
                        break;
                    }
                    else hiddenLayer[i] = new HiddenLayer(sizeHidden[i], sizeIN, inLayer, hiddenLayer[q], eps, alpha);
                }
                else
                {
                    if (q == hiddenLayer.Length)
                    {
                        hiddenLayer[i] = new HiddenLayer(sizeHidden[i], hiddenLayer[j].Size, hiddenLayer[j], outLayer, eps, alpha);
                        outLayer = new OutLayer(sizeOUT, hiddenLayer[i].Size, hiddenLayer[i], eps, alpha);
                        break;
                    }
                    else hiddenLayer[i] = new HiddenLayer(sizeHidden[i], hiddenLayer[j].Size, hiddenLayer[j], hiddenLayer[q], eps, alpha);
                }
            }
        }

        public double[] Calculate(params double[] inputSignal)
        {
            return inLayer.Calculate(inputSignal);
        }

        public void Learning()
        {
            lock (locker)
                LogString.Add("Начало обучения нейронной сети\n");

            double globalError = RequiredErrorSize + 1;
            int Epoch = 0;

            while (globalError > RequiredErrorSize)
            {
                globalError = 0;
                Epoch++;
                lock (locker)
                    LogString.Add("Эпоха: " + Epoch.ToString() + "\n");

                for (int i = 0; i < dataToTrain.TrainSet.Count; i++)
                {
                    Calculate(dataToTrain.TrainSet[i].InputSignal);
                    outLayer.Learning(dataToTrain.TrainSet[i].ExpectedResponse);
                }
                globalError = CalculateError();
                lock (locker)
                    LogString.Add("Эпоха: " + Epoch.ToString() + " размер ошибки: " + globalError.ToString() + "\n");
            }
            lock (locker)
                LogString.Add("Нейронная сеть успешно обучилась за " + Epoch.ToString() + " эпох.\n");
        }

        private double CalculateError()
        {
            double returnErrorValue = 0;
            double tempLocalErrorValue;
            double[] tempAnswerNetwork;

            for (int i = 0; i < dataToTrain.TrainSet.Count; i++)
            {
                tempAnswerNetwork = Calculate(dataToTrain.TrainSet[i].InputSignal);
                for (int j = 0; j < dataToTrain.SizeOut; j++)
                {
                    tempLocalErrorValue = Math.Pow((dataToTrain.TrainSet[i].ExpectedResponse[j] - tempAnswerNetwork[j]), 2) / dataToTrain.TrainSet.Count;
                    returnErrorValue += tempLocalErrorValue;
                }
            }
            return returnErrorValue;
        }

        public string[] GetLogInfo()
        {
            lock (locker)
            {
                string[] returnSring = LogString.ToArray();
                LogString.Clear();
                return returnSring;
            }
        }

        public string GetHiddenLayersInfo()
        {
            string returnSrting = "";

            for (int i = 0 , j = 1; i < hiddenLayer.Length; i++, j++)
            {
                returnSrting += hiddenLayer[i].Size.ToString();
                if (j != hiddenLayer.Length) returnSrting += ";";
            }
            return returnSrting;
        }
    }
}