using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NeuralNetworkUWP.Beta_NeuralNetwork_v3
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
                Synapse[i] = randDouble.NextDouble() - 0.5;
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



    public class HiddenNeuron : Neuron
    {
        private object nextLayer;

        public HiddenNeuron() : base() { }
        public HiddenNeuron(int nInput, int index, object nextLayer) : base(nInput, index)
        {
            this.nextLayer = nextLayer;
        }

        public void Learning()
        {
            double MultiplyDeltaSynapse = 0;
            if (nextLayer.Equals(typeof(HiddenLayer)))
            {
                var tempLayer = (HiddenLayer)nextLayer;
                for (int i = 0; i < tempLayer.Neuron.Length; i++)
                    MultiplyDeltaSynapse += tempLayer.Neuron[i].Delta * tempLayer.Neuron[i].Synapse[Index];
            }
            else
            {
                var tempLayer = (OutLayer)nextLayer;
                for (int i = 0; i < tempLayer.Neuron.Length; i++)
                    MultiplyDeltaSynapse += tempLayer.Neuron[i].Delta * tempLayer.Neuron[i].Synapse[Index];
            }

            Delta = (1 - Axon) * Axon * MultiplyDeltaSynapse;

            if (nextLayer.Equals(typeof(HiddenLayer)))
            {
                var tempLayer = (HiddenLayer)nextLayer;
                for (int i = 0; i < tempLayer.Neuron.Length; i++)
                    tempLayer.Neuron[i].RrecalculateSynapse(Index, tempLayer.Epsilon, tempLayer.Alpha);
                RrecalculateBias(tempLayer.Epsilon, tempLayer.Alpha);
            }
            else
            {
                var tempLayer = (OutLayer)nextLayer;
                for (int i = 0; i < tempLayer.Neuron.Length; i++)
                    tempLayer.Neuron[i].RrecalculateSynapse(Index, tempLayer.Epsilon, tempLayer.Alpha);
                RrecalculateBias(tempLayer.Epsilon, tempLayer.Alpha);
            }
        }
    }



    public class OutNeuron : Neuron
    {
        public OutNeuron() : base() { }
        public OutNeuron(int nInput, int index) : base(nInput, index) { }

        public void Learning(double idealAnswer)
        {
            Delta = (idealAnswer - Axon) * (1 - Axon) * Axon;
        }
    }



    public class InNeuron : Neuron
    {
        private object nextLayer;

        public InNeuron() : base() { }
        public InNeuron(int index, object nextLayer)
        {
            this.nextLayer = nextLayer;
        }

        public void Learning()
        {
            if (nextLayer.Equals(typeof(HiddenLayer)))
            {
                var tempLayer = (HiddenLayer)nextLayer;
                for (int i = 0; i < tempLayer.Neuron.Length; i++)
                    tempLayer.Neuron[i].RrecalculateSynapse(Index, tempLayer.Epsilon, tempLayer.Alpha);
            }
            else
            {
                var tempLayer = (OutLayer)nextLayer;
                for (int i = 0; i < tempLayer.Neuron.Length; i++)
                    tempLayer.Neuron[i].RrecalculateSynapse(Index, tempLayer.Epsilon, tempLayer.Alpha);
            }
        }

        public void Calculate(double inValue)
        {
            Axon = inValue;
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
