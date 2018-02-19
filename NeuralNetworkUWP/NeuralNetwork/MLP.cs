using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NeuralNetworkUWP.NeuralNetwork
{
    public class MLP
    {
        [XmlIgnore]
        private DataToTrain dataToTrain;
        [XmlArrayItem("LayerPerceptron")]
        public LayerPerceptron[] layerPerceptron;
        [XmlIgnore]
        private double requiredErrorSize;
        [XmlAttribute]
        public int sizeIN;
        [XmlAttribute]
        public int sizeOUT;
        [XmlIgnore]
        public List<string> LogString { get; }
        [XmlIgnore]
        object locker = new object();


        public MLP() { }

        public MLP(DataToTrain dataToTrain, params int[] numberOfHiddenLayers)
        {
            requiredErrorSize = 0.01;
            layerPerceptron = new LayerPerceptron[numberOfHiddenLayers.Length + 1];
            this.dataToTrain = dataToTrain;
            sizeIN = dataToTrain.SizeIn;
            sizeOUT = dataToTrain.SizeOut;

            LogString = new List<string>();

            if (layerPerceptron.Length == 1)
            {
                layerPerceptron[0] = new LayerPerceptron(1, sizeOUT);
            }
            else
            {
                layerPerceptron[0] = new LayerPerceptron(numberOfHiddenLayers[0], sizeIN);
                if (layerPerceptron.Length > 2)
                {
                    for (int i = 1; i < numberOfHiddenLayers.Length; i++)
                        layerPerceptron[i] = new LayerPerceptron(numberOfHiddenLayers[i], numberOfHiddenLayers[i - 1]);
                }
                layerPerceptron[numberOfHiddenLayers.Length] = new LayerPerceptron(sizeOUT, numberOfHiddenLayers[numberOfHiddenLayers.Length - 1]);
            }
        }

        public double Alpha
        {
            set
            {
                for (int i = 0; i < layerPerceptron.Length; i++)
                    layerPerceptron[i].Alpha = value;
            }
        }
        public double Epsilon
        {
            set
            {
                for (int i = 0; i < layerPerceptron.Length; i++)
                    layerPerceptron[i].Epsilon = value;
            }
        }

        public double Error
        {
            set
            {
                requiredErrorSize = value;
            }
        }

        public double[] Calculate(params double[] numb)
        {
            layerPerceptron[0].AxonOnPreviousLayer = numb;
            Calculate();
            double[] answerArray = new double[sizeOUT];
            answerArray = layerPerceptron[layerPerceptron.Length - 1].AxonLayer;
            return answerArray;
        }

        private void Calculate()
        {
            layerPerceptron[0].CalculateAxonOfLayer();
            for (int i = 1; i < layerPerceptron.Length; i++)
            {
                layerPerceptron[i].AxonOnPreviousLayer = (layerPerceptron[i - 1].AxonLayer);
                layerPerceptron[i].CalculateAxonOfLayer();
            }
        }

        private void CalculateLocalError(int indexTask)
        {
            double[] outputArray = new double[sizeOUT];
            outputArray = layerPerceptron[layerPerceptron.Length - 1].AxonLayer;

            double[] answerArray = new double[sizeOUT];
            answerArray = dataToTrain.TrainSet[indexTask].ExpectedResponse;

            double summ = 0;
            for (int i = 0; i < sizeOUT; i++)
                summ += Math.Pow((answerArray[i] - outputArray[i]), 2);

            lock (locker)
                LogString.Add("\tОбучающий сет: " + (indexTask + 1) + " размер локальной ошибки: " + (summ / dataToTrain.TrainSet.Count) + "\n");
        }

        private double CalculateGlobalError()
        {
            lock (locker)
                LogString.Add("Размер локальных ошибок после прохождения всех сетов:\n");

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
                    lock (locker) // TEst
                        LogString.Add("\tОбучающий сет: " + (i + 1) + " размер локальной ошибки: " + tempLocalErrorValue + "\n");
                }
            }
            return returnErrorValue;
        }

        public void Learning()
        {
            lock (locker)
                LogString.Add("Начало обучения нейронной сети\n");
            int counter = 0;
            double globalError = requiredErrorSize + 1;
            while (globalError > requiredErrorSize)
            {
                globalError = 0;
                counter++;
                lock (locker)
                    LogString.Add("Эпоха: " + counter.ToString() + "\n");

                for (int i = 0; i < dataToTrain.TrainSet.Count; i++)
                {
                    layerPerceptron[0].AxonOnPreviousLayer = dataToTrain.TrainSet[i].InputSignal;
                    Calculate();
                    layerPerceptron[layerPerceptron.Length - 1].LearningOutput(dataToTrain.TrainSet[i].ExpectedResponse);

                    for (int j = layerPerceptron.Length - 2; j >= 0; j--)
                        layerPerceptron[j].LearningHidden(layerPerceptron[j + 1].SummMultiply);
                    Calculate();
                    CalculateLocalError(i);
                }
                globalError = CalculateGlobalError();
                lock (locker)
                    LogString.Add("Эпоха: " + counter.ToString() + " размер ошибки: " + globalError.ToString() + "\n");
            }
            lock (locker)
                LogString.Add("Нейронная сеть успешно обучилась за " + counter.ToString() + " эпох.\n");
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
            string numberHiddenPerceptron = "";

            for (int i = 0; i < layerPerceptron.Length - 1; i++)
            {
                numberHiddenPerceptron += layerPerceptron[i].perceptron.Length.ToString();
                if (i != layerPerceptron.Length - 2) numberHiddenPerceptron += ";";
            }
            return numberHiddenPerceptron;
        }
    }
}

