using NeuralNetworkLibrary.NetworkStructure;
using System;

namespace NeuralNetworkLibrary
{
    public class NeuralNetworkComparer : IComparer<NeuralNetwork>
    {
        readonly Func<NeuralNetwork, double> fitnessFunction;

        public NeuralNetworkComparer(Func<NeuralNetwork, double> fitnessFunction)
        {
            this.fitnessFunction = fitnessFunction;
        }

        public int Compare(NeuralNetwork? x, NeuralNetwork? y)
        {
            double xFitness = fitnessFunction.Invoke(x);
            double yFitness = fitnessFunction.Invoke(y);

            return xFitness < yFitness ? -1 : (xFitness > yFitness ? 1 : 0);
        }
    }

    public class GeneticTrainer
    {
        public Random Random { get; set; }

        public double TopSurvivalThreshold { get; set; } = .10d;
        public double BottomSurvivalThreshold { get; set; } = .10d;

        public double MutationRate { get; set; }

        public Mutator Mutator { get; set; }

        NeuralNetwork[] networks;
        NeuralNetworkComparer comparer;

        double min;
        double max;

        public GeneticTrainer(Random random, int networkAmount, int[] neuronsPerLayer, double min, double max,
            ActivationFunction activationFunction, ErrorFunction errorFunction, Func<NeuralNetwork, double> fitnessFunction)
        {
            Random = random;

            this.min = min;
            this.max = max;

            networks = new NeuralNetwork[networkAmount];
            for(int i = 0; i < networkAmount; i++)
            {
                networks[i] = new NeuralNetwork(activationFunction, errorFunction, neuronsPerLayer);
                networks[i].Randomize(Random, min, max);
            }

            Mutator = new Mutator();

            MutationRate = 1;

            comparer = new NeuralNetworkComparer(fitnessFunction);
        }

        // layer crossover
        public void Crossover(Random random, int topCount, int bottomCount)
        {
            for (int i = topCount; i < networks.Length - bottomCount; i++)
            {
                int layerIndex = random.Next(1, networks[i].Layers.Length);

                Layer badLayer = networks[i].Layers[layerIndex];
                Layer goodLayer = networks[random.Next(0, topCount)].Layers[layerIndex];

                badLayer = new Layer(goodLayer.Neurons[0].ActivationFunc, goodLayer.Neurons.Length, previousLayer: networks[i].Layers[layerIndex - 1]);
            }
        }

        public void Train()
        {
            Array.Sort(networks, comparer);

            int topCount = (int)(networks.Length * TopSurvivalThreshold);
            int bottomCount = (int)(networks.Length * BottomSurvivalThreshold);

            Crossover(Random, topCount, bottomCount);

            for (int i = topCount; i < networks.Length - bottomCount; i++)
            {
                Mutator.Mutate(networks[i], Random, MutationRate);
            }

            for(int i = bottomCount; i < networks.Length; i++)
            {
                networks[i].Randomize(Random, min, max);
            }
        }
    }
}