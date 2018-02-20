using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworkUWP.Beta_NeuralNetwork_v3.Neuron;

namespace NeuralNetworkUWP.Beta_NeuralNetwork_v3.Neuron
{
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
            Type typeNextLayer = nextLayer.GetType();

            double MultiplyDeltaSynapse = 0;
            if (typeNextLayer.Equals(typeof(HiddenLayer)))
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

            if (typeNextLayer.Equals(typeof(HiddenLayer)))
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
}
