using NeuralNetworkLibrary.NetworkStructure;
using System.Net.NetworkInformation;
using System.Reflection;

using MonkeyNetwork = NeuralNetworkLibrary.NetworkStructure.NeuralNetwork;

namespace NeuralNetworkLibrary
{
    public class NeuralNetworkComparer : IComparer<NeuralNetwork>
    {
        readonly Func<NeuralNetwork, double> fitnessFunction;

        public int Compare(NeuralNetwork? x, NeuralNetwork? y)
            => x.Fitness > y.Fitness ? -1 : (x.Fitness < y.Fitness ? 1 : 0);

        //public int Compare(NeuralNetwork? x, NeuralNetwork? y)
        //{
        //    double xBias = x.Layers[0].Neurons[0].Bias;
        //    double yBias = y.Layers[0].Neurons[0].Bias;

        //    return xBias < yBias ? -1 : (xBias > yBias ? 1 : 0);
        //}
    }

    public class GeneticTrainer
    {
        static MonkeyNetwork monke;

        double bestFitness = double.NegativeInfinity;
        public Random Random { get; set; }

        public double TopSurvivalThreshold { get; set; } = .10d;
        public double BottomSurvivalThreshold { get; set; } = .10d;

        public double MutationRate { get; set; }

        public Mutator Mutator { get; set; }

        public NeuralNetwork[] Networks;
        NeuralNetworkComparer comparer;
        Func<NeuralNetwork, double> fitnessFunction;

        double min;
        double max;

        public GeneticTrainer(Random random, int networkAmount, int[] neuronsPerLayer, double min, double max, double mutationRate,
            ActivationFunction activationFunction, ErrorFunction errorFunction, Func<NeuralNetwork, double> fitnessFunction)
        {
            Random = random;

            this.min = min;
            this.max = max;

            Networks = new NeuralNetwork[networkAmount];
            for (int i = 0; i < networkAmount; i++)
            {
                Networks[i] = new NeuralNetwork(activationFunction, errorFunction, neuronsPerLayer);
                Networks[i].Randomize(Random, min, max);
            }

            Mutator = new Mutator();
            Mutator.PossibleMutations.Add(Mutator.PercentChange);
            Mutator.PossibleMutations.Add(Mutator.FlipSign);

            MutationRate = mutationRate;

            this.fitnessFunction = fitnessFunction;
            comparer = new NeuralNetworkComparer();
        }

        // layer crossover
        public void Crossover(Random random, int topCount, int bottomCount)
        {
            for (int netIndex = topCount; netIndex < Networks.Length - bottomCount; netIndex++)
            {
                NeuralNetwork goodNet = Networks[random.Next(0, 1)]; // <----- hardcoded to only crossover from top net
                NeuralNetwork badNet = Networks[netIndex];

                for(int layerIndex = 1; layerIndex < badNet.Layers.Length; layerIndex++)
                {
                    Layer goodLayer = goodNet.Layers[layerIndex];
                    Layer badLayer = badNet.Layers[layerIndex];

                    for (int neuronIndex = random.Next(0, badLayer.Neurons.Length); neuronIndex < badLayer.Neurons.Length; neuronIndex++)
                    {
                        Neuron goodNeuron = goodLayer.Neurons[neuronIndex];
                        Neuron badNeuron = badLayer.Neurons[neuronIndex];
                        
                        badNeuron.Bias = goodNeuron.Bias;

                        for(int dendriteIndex = 0; dendriteIndex < badNeuron.Dendrites.Length; dendriteIndex++)
                        {
                            badNeuron.Dendrites[dendriteIndex].Weight = goodNeuron.Dendrites[dendriteIndex].Weight;
                        }
                    }
                }
            }
        }

        public void Train()
        {
            for(int i = 0; i < Networks.Length; i++)
            {
                Networks[i].Fitness = fitnessFunction(Networks[i]);
            }

            Array.Sort(Networks, comparer);
            if (bestFitness < Networks[0].Fitness)
            {
                bestFitness = Networks[0].Fitness;                
            }
            int topCount = (int)(Networks.Length * TopSurvivalThreshold);
            int bottomCount = (int)(Networks.Length * BottomSurvivalThreshold);

            Crossover(Random, topCount, bottomCount);

            for (int i = topCount; i < Networks.Length - bottomCount; i++)
            {
                Mutator.Mutate(Networks[i], Random, MutationRate);
            }

            for (int i = Networks.Length - bottomCount; i < Networks.Length; i++)
            {
                Networks[i].Randomize(Random, min, max);
            }
        }
    }
}