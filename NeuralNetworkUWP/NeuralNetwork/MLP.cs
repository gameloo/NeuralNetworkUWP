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
        private double globalError;
        [XmlAttribute]
        public int sizeIN;
        [XmlAttribute]
        public int sizeOUT;
        [XmlIgnore]
        public List<string> LogString { get; }
        [XmlIgnore]
        object locker = new object();


        public MLP() { }

        public MLP(DataToTrain dataToTrain, params int[] _numberOfHiddenLayers)
        {
            globalError = 0.01;
            layerPerceptron = new LayerPerceptron[_numberOfHiddenLayers.Length + 1];
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
                layerPerceptron[0] = new LayerPerceptron(_numberOfHiddenLayers[0], sizeIN);
                if (layerPerceptron.Length > 2)
                {
                    for (int i = 1; i < _numberOfHiddenLayers.Length; i++)
                        layerPerceptron[i] = new LayerPerceptron(_numberOfHiddenLayers[i], _numberOfHiddenLayers[i - 1]);
                }
                layerPerceptron[_numberOfHiddenLayers.Length] = new LayerPerceptron(sizeOUT, _numberOfHiddenLayers[_numberOfHiddenLayers.Length - 1]);
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
                globalError = value;
            }
        }

        public double[] calculate(params double[] _numb)
        {
            layerPerceptron[0].AxonOnPreviousLayer = _numb;
            calculate();
            double[] answerArray = new double[sizeOUT];
            answerArray = layerPerceptron[layerPerceptron.Length - 1].AxonLayer;
            return answerArray;
        }

        private void calculate()
        {
            layerPerceptron[0].calculateAxonOfLayer();
            for (int i = 1; i < layerPerceptron.Length; i++)
            {
                layerPerceptron[i].AxonOnPreviousLayer = (layerPerceptron[i - 1].AxonLayer);
                layerPerceptron[i].calculateAxonOfLayer();
            }
        }

        private double calculateError(int _indexTask)
        {
            double[] outputArray = new double[sizeOUT];
            outputArray = layerPerceptron[layerPerceptron.Length - 1].AxonLayer;

            double[] answerArray = new double[sizeOUT];
            answerArray = dataToTrain.TrainSet[_indexTask].ExpectedResponse;

            double summ = 0;
            for (int i = 0; i < sizeOUT; i++)
                summ += Math.Pow((answerArray[i] - outputArray[i]), 2);
            //return summ / sizeOUT;
            return summ; // Эксперимент 
        }

        public void learning()
        {
            lock (locker)
                LogString.Add("Начало обучения нейронной сети\n");
            int counter = 0;
            double error = globalError + 1;
            while (error > globalError)
            {
                error = 0;
                counter++;
                for (int i = 0; i < dataToTrain.TrainSet.Count; i++)
                {
                    layerPerceptron[0].AxonOnPreviousLayer = dataToTrain.TrainSet[i].InputSignal;
                    calculate();
                    layerPerceptron[layerPerceptron.Length - 1].learningOutput(dataToTrain.TrainSet[i].ExpectedResponse);

                    for (int j = layerPerceptron.Length - 2; j >= 0; j--)
                        layerPerceptron[j].learningHidden(layerPerceptron[j + 1].SummMultiply);
                    calculate();
                    error += calculateError(i);
                }
                lock (locker)
                    LogString.Add("Итерация:" + counter.ToString() + " размер ошибки: " + error.ToString() + "\n");
            }
            lock (locker)
                LogString.Add("Нейронная сеть успешно обучилась за " + counter.ToString() + " итераций.\n");
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
    }
}

