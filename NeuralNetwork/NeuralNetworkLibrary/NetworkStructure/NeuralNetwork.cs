namespace NeuralNetworkLibrary.NetworkStructure
{
    public class NeuralNetwork
    {
        public Layer[] Layers { get; private set; }
        public double[] Inputs
        {
            get => Layers[0].Neurons.Select((Neuron n) => n.ActivatedOutput).ToArray();
            private set
            {
                for(int i = 0; i < Layers[0].Neurons.Length; i++)
                {
                    Layers[0].Neurons[i].ActivatedOutput = value[i];
                }
            }
        }
        public double[] Outputs
        {
            get => Layers[^1].Neurons.Select((Neuron n) => n.ActivatedOutput).ToArray();
            private set
            {
                for (int i = 0; i < Layers[0].Neurons.Length; i++)
                {
                    Layers[^1].Neurons[i].ActivatedOutput = value[i];
                }
            }
        }

        ErrorFunction errorFunc;

        public NeuralNetwork(ActivationFunction activationFunc, ErrorFunction errorFunc, params int[] neuronsPerLayer)
        {
            this.errorFunc = errorFunc;

            Inputs = new double[neuronsPerLayer[0]];

            Layers = new Layer[neuronsPerLayer.Length];
            Layers[0] = new Layer(ActivationFunction.Identity, neuronsPerLayer[0], null);

            Layer prevLayer = Layers[0];

            for (int i = 1; i < neuronsPerLayer.Length; i++)
            {
                Layers[i] = new Layer(activationFunc, neuronsPerLayer[i], prevLayer);
                prevLayer = Layers[i];
            }
        }

        public void Randomize(Random random, double min, double max)
        {
            foreach(Layer layer in Layers)
            {
                layer.Randomize(random, min, max);
            }
        }

        public double[] Compute(double[] inputs)
        {
            Inputs = inputs;
            for(int i = 1; i < Layers.Length; i++)
            {
                Layers[i].Compute();
            }
            return Outputs;
        }

        public double GetError(double[] outputs, double[] desiredOutputs)
        {
            double sum = 0;

            for(int i = 0; i < outputs.Length; i++)
            {
                sum += errorFunc.Function(outputs[i], desiredOutputs[i]);
            }
            return sum;
        }
    }
}
