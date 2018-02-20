using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkUWP.Beta_NeuralNetwork_v3.Neuron
{
    public class OutNeuron : Neuron
    {
        public OutNeuron() : base() { }
        public OutNeuron(int nInput, int index) : base(nInput, index) { }

        public void Learning(double idealAnswer)
        {
            Delta = (idealAnswer - Axon) * (1 - Axon) * Axon;
        }
    }
}
