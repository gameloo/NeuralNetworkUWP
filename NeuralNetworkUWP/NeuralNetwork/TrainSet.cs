using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkUWP.NeuralNetwork
{
    public class TrainSet
    {
        public double[] InputSignal { set; get; } // Сигналы на входы нейронной сети
        public double[] ExpectedResponse { set; get; } // Ожидаемый ответ от сети
    }
}
