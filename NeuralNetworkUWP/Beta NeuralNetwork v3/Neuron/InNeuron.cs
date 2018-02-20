using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkUWP.Beta_NeuralNetwork_v3.Neuron
{
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
            Type typeNextLayer = nextLayer.GetType();

            if (typeNextLayer.Equals(typeof(HiddenLayer)))
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
}
