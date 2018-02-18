using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NeuralNetworkUWP.NeuralNetwork
{
    public class LayerPerceptron
    {
        [XmlArrayItem("Perceptron")]
        public Perceptron[] perceptron;
        [XmlIgnore]
        public double[] AxonOnPreviousLayer { set; private get; }
        [XmlIgnore]
        public double[] SummMultiply { private set; get; }

        public LayerPerceptron() { }

        public LayerPerceptron(int _numberOfPerceptrons, int _numberOfPerceptronsOnPreviousLayer)
        {
            perceptron = new Perceptron[_numberOfPerceptrons];
            for (int i = 0; i < perceptron.Length; i++)
                perceptron[i] = new Perceptron(_numberOfPerceptronsOnPreviousLayer);
            AxonOnPreviousLayer = new double[_numberOfPerceptronsOnPreviousLayer];
            SummMultiply = new double[_numberOfPerceptronsOnPreviousLayer];
        }

        public double Alpha
        {
            set
            {
                for (int i = 0; i < perceptron.Length; i++) perceptron[i].Alpha = value;
            }
        }

        public double Epsilon
        {
            set
            {
                for (int i = 0; i < perceptron.Length; i++) perceptron[i].Epsilon = value;
            }
        }

        public double[] AxonLayer
        {
            get
            {
                double[] returnArray = new double[perceptron.Length];
                for (int i = 0; i < returnArray.Length; i++)
                    returnArray[i] = perceptron[i].Axon;
                return returnArray;
            }
        }

        public void calculateAxonOfLayer()
        {
            for (int i = 0; i < perceptron.Length; i++)
            {
                perceptron[i].AxonTerminal = AxonOnPreviousLayer;
                perceptron[i].calculateAxon();
            }
        }

        public void learningOutput(double[] _idealAnswer)
        {
            for (int i = 0; i < perceptron.Length; i++)
            {
                perceptron[i].calcDeltaOUT(_idealAnswer[i]);
                perceptron[i].changeWeight();
            }
            setSummMultiply();
        }

        public void learningHidden(double[] _summMultiply)
        {
            for (int i = 0; i < perceptron.Length; i++)
            {
                perceptron[i].calcDeltaHIDDEN(_summMultiply[i]);
                perceptron[i].changeWeight();
            }
            setSummMultiply();
        }

        private void setSummMultiply()
        {
            for (int i = 0; i < AxonOnPreviousLayer.Length; i++)
                SummMultiply[i] = calculateSummMultiply(i);
        }

        private double calculateSummMultiply(int _index)
        {
            double returnValue = 0;
            for (int i = 0; i < perceptron.Length; i++)
                returnValue += perceptron[i].getMultiplyDeltaWeight(_index);
            return returnValue;
        }
    }
}

