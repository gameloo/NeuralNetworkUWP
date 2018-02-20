using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NeuralNetworkUWP.Beta_NeuralNetwork_v3.Neuron
{
    public abstract class Neuron
    {
        [XmlArrayItem("Synapse")]
        public double[] Synapse { get; protected set; } // Веса синапсов.

        [XmlIgnore]
        public double[] Input { set; protected get; } // Значения на входы.

        [XmlIgnore]
        public double Axon { protected set; get; } // Выход нейрона.

        [XmlElement("Bias")]
        public double Bias { protected set; get; } // Вес синапса нейрона смещения.

        [XmlIgnore]
        public double Delta { protected set; get; } // Дельта нейрона.

        [XmlAttribute("Index")]
        public int Index { protected set; get; } // Индекс нейрона.

        [XmlIgnore]
        protected double[] deltaSynapse;

        [XmlIgnore]
        protected double deltaBias;

        public Neuron() { }

        public Neuron(int nInput, int index)
        {
            Random randDouble = new Random((int)DateTime.Now.Ticks);

            Synapse = new double[nInput];
            Input = new double[nInput];
            deltaSynapse = new double[nInput];
            Bias = randDouble.NextDouble() - 0.5;
            Index = index;

            for (int i = 0; i < nInput; i++)
            {
                Synapse[i] = randDouble.NextDouble()*2 - 0.5;
                Task.Delay(TimeSpan.FromTicks(randDouble.Next(1, 100)));

                deltaSynapse[i] = 0;
            }

            deltaBias = 0;
        }

        public void Calculate()
        {
            double summWeight = 0;
            for (int i = 0; i < Synapse.Length; i++)
                summWeight += Synapse[i] * Input[i];
            Axon = MyMath.Sigmoid(summWeight + Bias);
        }

        public void RrecalculateSynapse(int iSynapse, double eps, double alpha)
        {
            double GRAD = Delta * Synapse[iSynapse];
            deltaSynapse[iSynapse] = eps * GRAD + deltaSynapse[iSynapse] * alpha;
            Synapse[iSynapse] += deltaSynapse[iSynapse];
        }

        public void RrecalculateBias(double eps, double alpha)
        {
            double GRAD = Delta * Bias;
            deltaBias = eps* GRAD + deltaBias* alpha;
            Bias += deltaBias;
        }
    }



    



  



   



    public static class MyMath
    {
        public static double Sigmoid(double x)
        {
            double expValue = Math.Exp(x * (-1));
            return 1 / (1 + expValue);
        }
    }
}
