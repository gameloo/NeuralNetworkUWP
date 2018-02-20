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

        public HiddenLayer(int size, int sizePrevious, object previousLayer, object nextLayer, double eps, double alpha)
        {
            Epsilon = eps;
            Alpha = alpha;
            Size = size;
            this.previousLayer = previousLayer;
            this.nextLayer = nextLayer;
            Neuron = new HiddenNeuron[size];
            for (int i = 0; i < size; i++)
                Neuron[i] = new HiddenNeuron(sizePrevious, i, nextLayer);
        }

        public override double[] GetOutputSignal() 
        {
            double[] returnSignal = new double[Size];
            for(int i = 0; i < Size; i++)
            {
                returnSignal[i] = Neuron[i].Axon; 
            }
            return returnSignal;
        }


        public double[] Calculate()
        {
            for (int i = 0; i < Size; i++)
            {
                if (nextLayer.Equals(typeof(HiddenLayer)))
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

            if (nextLayer.Equals(typeof(HiddenLayer)))
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
            for (int i =0; i < Size; i++)
            {
                Neuron[i].Learning();
            }
        }
    } 


    public class InLayer :LayerNeuron
    {
        [XmlArrayItem("InNeuron")]
        public InNeuron[] Neuron { get; private set; }

        public InLayer() : base() { }

        public InLayer(int size, object nextLayer, double eps, double alpha)
        {
            Epsilon = eps;
            Alpha = alpha;
            Size = size;
            this.nextLayer = nextLayer;
            Neuron = new InNeuron[size];
            for (int i = 0; i < size; i++)
                Neuron[i] = new InNeuron(i, nextLayer);
        }

        public double[] Calculate (double[] inputSignal)
        {
            for (int i = 0; i < Neuron.Length; i++)
            {
                Neuron[i].Calculate(inputSignal[i]);
            }
            if (nextLayer.Equals(typeof(HiddenLayer)))
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

        public OutLayer(int size, int nPreviousNeurons, object previousLayer, double eps, double alpha)
        {
            Epsilon = eps;
            Alpha = alpha;
            Size = size;
            this.previousLayer = previousLayer;
            Neuron = new OutNeuron[size];
            for (int i = 0; i < size; i++)
                Neuron[i] = new OutNeuron(nPreviousNeurons, i); // Добавить указатель на предыдущий слой, но смысл?
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
            for (int i = 0; i < Size; i++)
            {
                if (nextLayer.Equals(typeof(HiddenLayer)))
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
        }


    }
}
