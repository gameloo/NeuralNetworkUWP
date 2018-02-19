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

        public LayerPerceptron(int numberOfPerceptrons, int numberOfPerceptronsOnPreviousLayer)
        {
            perceptron = new Perceptron[numberOfPerceptrons];
            for (int i = 0; i < perceptron.Length; i++)
                perceptron[i] = new Perceptron(numberOfPerceptronsOnPreviousLayer);
            AxonOnPreviousLayer = new double[numberOfPerceptronsOnPreviousLayer];
            SummMultiply = new double[numberOfPerceptronsOnPreviousLayer];
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

        public void CalculateAxonOfLayer()
        {
            for (int i = 0; i < perceptron.Length; i++)
            {
                perceptron[i].AxonTerminal = AxonOnPreviousLayer;
                perceptron[i].CalculateAxon();
            }
        }

        public void LearningOutput(double[] idealAnswer)
        {
            for (int i = 0; i < perceptron.Length; i++)
            {
                perceptron[i].CalcDeltaOUT(idealAnswer[i]);
                perceptron[i].ChangeWeight();
            }
            SetSummMultiply();
        }

        public void LearningHidden(double[] summMultiply)
        {
            for (int i = 0; i < perceptron.Length; i++)
            {
                perceptron[i].CalcDeltaHIDDEN(summMultiply[i]);
                perceptron[i].ChangeWeight();
            }
            SetSummMultiply();
        }

        private void SetSummMultiply()
        {
            for (int i = 0; i < AxonOnPreviousLayer.Length; i++)
                SummMultiply[i] = CalculateSummMultiply(i);
        }

        private double CalculateSummMultiply(int index)
        {
            double returnValue = 0;
            for (int i = 0; i < perceptron.Length; i++)
                returnValue += perceptron[i].GetMultiplyDeltaWeight(index);
            return returnValue;
        }
    }
}

