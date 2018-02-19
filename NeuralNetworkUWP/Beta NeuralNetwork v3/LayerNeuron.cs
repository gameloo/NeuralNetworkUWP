using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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

        public LayerNeuron() { }
    }

    public class HiddenLayer : LayerNeuron
    {
        [XmlArrayItem("HiddenNeuron")]
        public HiddenNeuron[] Neuron { get; private set; }

        public HiddenLayer() : base() { }

        public HiddenLayer(int nNeuron, int nPreviousNeurons, object previousLayer, object nextLayer, double eps, double alpha)
        {
            Epsilon = eps;
            Alpha = alpha;
            this.previousLayer = previousLayer;
            this.nextLayer = nextLayer;
            Neuron = new HiddenNeuron[nNeuron];
            for (int i = 0; i < nNeuron; i++)
                Neuron[i] = new HiddenNeuron(nPreviousNeurons, i, nextLayer);
        }




    } 

    public class OutLayer : LayerNeuron
    {
        [XmlArrayItem("OutNeuron")]
        public OutNeuron[] Neuron { get; private set; }

        public OutLayer() : base() { }

        public OutLayer(int nNeuron, int nPreviousNeurons, object previousLayer, double eps, double alpha)
        {
            Epsilon = eps;
            Alpha = alpha;
            this.previousLayer = previousLayer;
            Neuron = new OutNeuron[nNeuron];
            for (int i = 0; i < nNeuron; i++)
                Neuron[i] = new OutNeuron(nPreviousNeurons, i); // Добавить указатель на предыдущий слой
        }

        




    }

    public class InLayer :LayerNeuron
    {
        [XmlArrayItem("InNeuron")]
        public InNeuron[] Neuron { get; private set; }

        public InLayer() : base() { }

        public InLayer(int nInput, object nextLayer, double eps, double alpha)
        {
            Epsilon = eps;
            Alpha = alpha;
            this.nextLayer = nextLayer;
            Neuron = new InNeuron[nInput];
            for (int i = 0; i < nInput; i++)
                Neuron[i] = new InNeuron(i, nextLayer);
        }
    }
}
