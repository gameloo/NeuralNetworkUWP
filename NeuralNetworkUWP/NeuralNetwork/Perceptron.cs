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

        public Perceptron(int _numberOfDendrites)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            weightDendrite = new double[_numberOfDendrites];
            for (int i = 0; i < weightDendrite.Length; i++)
            {
                weightDendrite[i] = rand.Next(-100, 100) * 0.01;
            }
            activWeight = rand.Next(-100, 100) * 0.01;
            AxonTerminal = new double[_numberOfDendrites];
            deltaWeight = new double[_numberOfDendrites + 1];
            for (int i = 0; i < deltaWeight.Length; i++)
            {
                deltaWeight[i] = 0;
            }
            Alpha = 0.3;
            Epsilon = 0.3;
        }

        private double sigmoid(double _x)
        {
            double expValue;
            expValue = Math.Exp((double)-_x);
            return 1 / (1 + expValue);
        }

        public void calculateAxon()
        {
            Axon = 0;
            for (int i = 0; i < weightDendrite.Length; i++)
                Axon += weightDendrite[i] * AxonTerminal[i];
            Axon += activWeight;
            Axon = sigmoid(Axon);
        }

        public void calcDeltaOUT(double _idealAnswer)
        {
            delta = (_idealAnswer - Axon) * (1 - Axon) * Axon;
        }

        public void calcDeltaHIDDEN(double _summDelta)
        {
            delta = (1 - Math.Pow(Axon, 2)) * _summDelta;
        }

        private double GRAD(int _indexAxonTerminal)
        {
            return delta * AxonTerminal[_indexAxonTerminal];
        }

        public void changeWeight()
        {
            for (int i = 0; i < weightDendrite.Length; i++)
            {
                deltaWeight[i] = Epsilon * GRAD(i) + Alpha * deltaWeight[i];
                weightDendrite[i] = weightDendrite[i] + deltaWeight[i];
            }
            deltaWeight[weightDendrite.Length] = Epsilon * delta + Alpha * deltaWeight[weightDendrite.Length];
            activWeight = activWeight + deltaWeight[weightDendrite.Length];
        }

        public double getMultiplyDeltaWeight(int _index)
        {
            return weightDendrite[_index] * delta;
        }
    }
}

