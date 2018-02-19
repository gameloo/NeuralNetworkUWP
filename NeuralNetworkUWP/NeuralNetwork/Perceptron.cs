using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NeuralNetworkUWP.NeuralNetwork
{
    public class Perceptron
    {
        [XmlIgnore]
        public double[] AxonTerminal { set; private get; }  //
        [XmlArrayItem("WeightDendrite")]
        public double[] weightDendrite;  // 
        [XmlIgnore]
        private double[] deltaWeight;     // 
        [XmlIgnore]
        public double Axon { private set; get; }    // Аксон (выход нейрона)
        [XmlElement("ActiveWeight")]
        public double activWeight;            // Вес активации
        [XmlIgnore]
        private double delta;
        [XmlAttribute("Alpha")]
        public double Alpha { set; get; }      // Момент обучения
        [XmlAttribute("Epsilon")]
        public double Epsilon { set; get; }    // Коэффициент скорости обучения

        public Perceptron() { }

        public Perceptron(int numberOfDendrites)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            weightDendrite = new double[numberOfDendrites];
            for (int i = 0; i < weightDendrite.Length; i++)
            {
                weightDendrite[i] = rand.Next(-100, 100) * 0.01;
            }
            activWeight = rand.Next(-100, 100) * 0.01;
            AxonTerminal = new double[numberOfDendrites];
            deltaWeight = new double[numberOfDendrites + 1];
            for (int i = 0; i < deltaWeight.Length; i++)
            {
                deltaWeight[i] = 0;
            }
        }

        private double Sigmoid(double x)
        {
            double expValue;
            expValue = Math.Exp(x*(-1));
            return 1 / (1 + expValue);
        }

        public void CalculateAxon()
        {
            Axon = 0;
            for (int i = 0; i < weightDendrite.Length; i++)
                Axon += weightDendrite[i] * AxonTerminal[i];
            Axon += activWeight;
            Axon = Sigmoid(Axon);
        }

        public void CalcDeltaOUT(double idealAnswer)
        {
            delta = (idealAnswer - Axon) * (1 - Axon) * Axon;
        }

        public void CalcDeltaHIDDEN(double summDelta)
        {
            delta = (1 - Math.Pow(Axon, 2)) * summDelta;
        }

        private double GRAD(int indexAxonTerminal)
        {
            return delta * AxonTerminal[indexAxonTerminal];
        }

        public void ChangeWeight()
        {
            for (int i = 0; i < weightDendrite.Length; i++)
            {
                deltaWeight[i] = Epsilon * GRAD(i) + Alpha * deltaWeight[i];
                weightDendrite[i] = weightDendrite[i] + deltaWeight[i];
            }
            deltaWeight[weightDendrite.Length] = Epsilon * delta + Alpha * deltaWeight[weightDendrite.Length];
            activWeight = activWeight + deltaWeight[weightDendrite.Length];
        }

        public double GetMultiplyDeltaWeight(int index)
        {
            return weightDendrite[index] * delta;
        }
    }
}

