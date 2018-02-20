using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NeuralNetworkUWP.Beta_NeuralNetwork_v3.Neuron;

namespace NeuralNetworkUWP.Beta_NeuralNetwork_v3
{
    abstract public class LayerNeuron
    {
        [XmlIgnore]
        public double Alpha { get; protected set; }

        [XmlIgnore]
        public double Epsilon { get; protected set; }

        [XmlIgnore]
        protected object previousLayer;

        [XmlIgnore]
        protected object nextLayer;

        [XmlAttribute]
        public int Size { get; protected set; }

        public LayerNeuron() { }

        public abstract double[] GetOutputSignal(); // Пофиксить, м.б. использовать интерфейс?

    }

    public class HiddenLayer : LayerNeuron
    {
        [XmlArrayItem("HiddenNeuron")]
        public HiddenNeuron[] Neuron { get; private set; }

        public HiddenLayer() : base() { }

        public HiddenLayer(int size, double eps, double alpha)
        {
            Epsilon = eps;
            Alpha = alpha;
            Size = size;
            Neuron = new HiddenNeuron[size];
        }

        public void ConfigureNeurons(object previousLayer, object nextLayer)
        {
            this.previousLayer = previousLayer;
            this.nextLayer = nextLayer;

            int sizePrevious;
            Type typePreviousLayer = this.previousLayer.GetType();
            if (typePreviousLayer.Equals(typeof(HiddenLayer)))
            {
                var tempLayer = (HiddenLayer)previousLayer;
                sizePrevious = tempLayer.Size;
            }
            else
            {
                var tempLayer = (InLayer)previousLayer;
                sizePrevious = tempLayer.Size;
            }

            for (int i = 0; i < Size; i++)
                Neuron[i] = new HiddenNeuron(sizePrevious, i, nextLayer);
        }

        public override double[] GetOutputSignal()
        {
            double[] returnSignal = new double[Size];
            for (int i = 0; i < Size; i++)
            {
                returnSignal[i] = Neuron[i].Axon;
            }
            return returnSignal;
        }


        public double[] Calculate()
        {
            Type typePreviousLayer = previousLayer.GetType();
            Type typeNextLayer = nextLayer.GetType();

            for (int i = 0; i < Size; i++)
            {
                if (typePreviousLayer.Equals(typeof(HiddenLayer)))
                {
                    var tempLayer = (HiddenLayer)previousLayer;
                    Neuron[i].Input = tempLayer.GetOutputSignal();
                }
                else
                {
                    var tempLayer = (InLayer)previousLayer;
                    Neuron[i].Input = tempLayer.GetOutputSignal();
                }
                Neuron[i].Calculate();
            }

            if (typeNextLayer.Equals(typeof(HiddenLayer)))
            {
                var tempLayer = (HiddenLayer)nextLayer;
                return tempLayer.Calculate();
            }
            else
            {
                var tempLayer = (OutLayer)nextLayer;
                return tempLayer.Calculate();
            }
        }

        public void Learning()
        {
            for (int i = 0; i < Size; i++)
            {
                Neuron[i].Learning();
            }

            Type typePreviousLayer = previousLayer.GetType();
            if (typePreviousLayer.Equals(typeof(HiddenLayer)))
            {
                var tempLayer = (HiddenLayer)previousLayer;
                tempLayer.Learning();
            }
            else
            {
                var tempLayer = (InLayer)previousLayer;
                tempLayer.Learning();
            }
        }
    }


    public class InLayer : LayerNeuron
    {
        [XmlArrayItem("InNeuron")]
        public InNeuron[] Neuron { get; private set; }

        public InLayer() : base() { }

        public InLayer(int size, double eps, double alpha)
        {
            Epsilon = eps;
            Alpha = alpha;
            Size = size;
            Neuron = new InNeuron[size];
        }

        public void ConfigureNeurons(object nextLayer)
        {
            this.nextLayer = nextLayer;
            for (int i = 0; i < Size; i++)
                Neuron[i] = new InNeuron(i, nextLayer);
        }

        public double[] Calculate(double[] inputSignal)
        {
            Type typeNextLayer = nextLayer.GetType();

            for (int i = 0; i < Neuron.Length; i++)
            {
                Neuron[i].Calculate(inputSignal[i]);
            }

            if (typeNextLayer.Equals(typeof(HiddenLayer)))
            {
                var tempLayer = (HiddenLayer)nextLayer;
                return tempLayer.Calculate();
            }
            else
            {
                var tempLayer = (OutLayer)nextLayer;
                return tempLayer.Calculate();
            }
        }

        public override double[] GetOutputSignal()
        {
            double[] returnSignal = new double[Size];
            for (int i = 0; i < Size; i++)
            {
                returnSignal[i] = Neuron[i].Axon;
            }
            return returnSignal;
        }

        public void Learning()
        {
            for (int i = 0; i < Size; i++)
                Neuron[i].Learning();
        }
    }


    public class OutLayer : LayerNeuron
    {
        [XmlArrayItem("OutNeuron")]
        public OutNeuron[] Neuron { get; private set; }

        public OutLayer() : base() { }

        public OutLayer(int size, double eps, double alpha)
        {
            Epsilon = eps;
            Alpha = alpha;
            Size = size;
            Neuron = new OutNeuron[size];
        }

        public void ConfigureNeurons(object previousLayer)
        {
            this.previousLayer = previousLayer;

            Type typePreviousLayer = previousLayer.GetType();
            int sizePrevious;
            if (typePreviousLayer.Equals(typeof(HiddenLayer)))
            {
                var tempLayer = (HiddenLayer)previousLayer;
                sizePrevious = tempLayer.Size;
            }
            else
            {
                var tempLayer = (InLayer)previousLayer;
                sizePrevious = tempLayer.Size;
            }

            for (int i = 0; i < Size; i++)
                Neuron[i] = new OutNeuron(sizePrevious, i);
        }

        public override double[] GetOutputSignal()
        {
            double[] returnSignal = new double[Size];
            for (int i = 0; i < Size; i++)
            {
                returnSignal[i] = Neuron[i].Axon;
            }
            return returnSignal;
        }

        public double[] Calculate()
        {
            Type typePreviousLayer = previousLayer.GetType();
            for (int i = 0; i < Size; i++)
            {
                if (typePreviousLayer.Equals(typeof(HiddenLayer)))
                {
                    var tempLayer = (HiddenLayer)previousLayer;
                    Neuron[i].Input = tempLayer.GetOutputSignal();
                }
                else
                {
                    var tempLayer = (InLayer)previousLayer;
                    Neuron[i].Input = tempLayer.GetOutputSignal();
                }
                Neuron[i].Calculate();
            }

            return GetOutputSignal();
        }

        public void Learning(double[] idealAnswer)
        {

            for (int i = 0; i < Size; i++)
                Neuron[i].Learning(idealAnswer[i]);

            Type typePreviousLayer = previousLayer.GetType();
            if (typePreviousLayer.Equals(typeof(HiddenLayer)))
            {
                var tempLayer = (HiddenLayer)previousLayer;
                tempLayer.Learning();
            }
            else
            {
                var tempLayer = (InLayer)previousLayer;
                tempLayer.Learning();
            }


        }


    }
}
