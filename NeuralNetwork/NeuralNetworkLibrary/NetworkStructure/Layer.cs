namespace NeuralNetworkLibrary.NetworkStructure
{
    public class Layer
    {
        public Neuron[] Neurons { get; }
        public double[] Outputs { get; }

        public Layer(ActivationFunction activation, int neuronCount, Layer previousLayer)
        {
            Neurons = new Neuron[neuronCount];

            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i] = new Neuron(activation, previousLayer.Neurons);
            }

            Outputs = new double[Neurons.Length];
        }

        public void Randomize(Random random, double min, double max)
        {
            foreach(var neuron in Neurons)
            {
                neuron.Randomize(random, min, max);
            }
        }

        public double[] Compute()
        {
            for(int i = 0; i < Outputs.Length; i++)
            {
                Outputs[i] = Neurons[i].Compute();
            }
            return Outputs;
        }
    }
}