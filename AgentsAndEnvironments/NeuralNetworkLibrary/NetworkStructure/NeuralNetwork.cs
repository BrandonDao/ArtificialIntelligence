using System.Diagnostics;

namespace NeuralNetworkLibrary.NetworkStructure
{
    [DebuggerDisplay("fitness: {Fitness}")]
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

        private readonly ErrorFunction errorFunc;
        private readonly ActivationFunction activationFunc;
        private readonly int[] neuronsPerLayer;

        public double Fitness;

        public NeuralNetwork(ActivationFunction activationFunc, ErrorFunction errorFunc, params int[] neuronsPerLayer)
        {
            this.activationFunc = activationFunc;
            this.errorFunc = errorFunc;
            this.neuronsPerLayer = neuronsPerLayer;

            Layers = new Layer[neuronsPerLayer.Length];
            Layers[0] = new Layer(ActivationFunction.Identity, neuronsPerLayer[0]);

            Inputs = new double[neuronsPerLayer[0]];

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

        public void Backprop(double learningRate, double[] desiredOutputs)
        {
            Layer outputLayer = Layers[^1];

            for(int i = 0; i < outputLayer.Neurons.Length; i++)
            {
                outputLayer.Neurons[i].Delta = errorFunc.Derivative(outputLayer.Neurons[i].ActivatedOutput, desiredOutputs[i]);
            }

            for(int i = Layers.Length - 1; i > 0; i--)
            {
                Layers[i].Backprop(learningRate);
            }
        }

        public void ApplyUpdates()
        {
            for(int i = 1; i < Layers.Length; i++)
            {
                Layers[i].ApplyUpdates();
            }
        }

        public NeuralNetwork Clone()
        {
            NeuralNetwork clone = new(activationFunc, errorFunc, neuronsPerLayer);

            for(int layerIdx = 0; layerIdx < Layers.Length; layerIdx++)
            {
                for(int neuronIdx = 0; neuronIdx < Layers[layerIdx].Neurons.Length; neuronIdx++)
                {
                    for(int dendriteIdx = 0; dendriteIdx < Layers[layerIdx].Neurons[neuronIdx].Dendrites.Length; dendriteIdx++)
                    {
                        clone.Layers[layerIdx].Neurons[neuronIdx].Dendrites[dendriteIdx].Weight = Layers[layerIdx].Neurons[neuronIdx].Dendrites[dendriteIdx].Weight;
                    }
                }
            }
            return clone;
        }

        public double TrainWithGradientDescent(double[][] inputs, double[][] desiredOutputs, double learningRate)
        {
            double summedError = 0;

            for(int i = 0; i < inputs.Length; i++)
            {
                var outputs = Compute(inputs[i]);
                summedError += GetError(outputs, desiredOutputs[i]);

                Backprop(learningRate, desiredOutputs[i]);
            }

            ApplyUpdates();

            return summedError / inputs.Length;
        }
    }
}
