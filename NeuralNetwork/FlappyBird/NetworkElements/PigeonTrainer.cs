using NeuralNetworkLibrary.NetworkStructure;
using System;
using System.Collections.Generic;

namespace FlappyBird.NetworkElements
{
    public class PigeonComparer : IComparer<Pigeon>
    {
        public int Compare(Pigeon x, Pigeon y)
            => x.Score > y.Score ? -1 : x.Score < y.Score ? 1 : 0;
    }

    public class PigeonTrainer
    {
        double bestFitness = double.NegativeInfinity;
        public Random Random { get; set; }

        public double TopSurvivalThreshold { get; set; } = .03d;
        public double BottomSurvivalThreshold { get; set; } = .10d;

        public double MutationRate { get; set; }

        public Mutator Mutator { get; set; }

        public Pigeon[] Pigeons;
        
        
        private readonly PigeonComparer comparer;

        private readonly double min;
        private readonly double max;

        public PigeonTrainer(Random random, Pigeon[] networks, double mutationRate, double min, double max)
        {
            Random = random;

            Pigeons = networks;
            foreach (var pigeon in Pigeons)
            {
                pigeon.Network.Randomize(random, min, max);
            }

            this.min = min;
            this.max = max;

            Mutator = new Mutator();
            Mutator.PossibleMutations.Add(Mutator.PercentChange);
            Mutator.PossibleMutations.Add(Mutator.FlipSign);

            MutationRate = mutationRate;

            comparer = new PigeonComparer();
        }

        // layer crossover
        public void Crossover(Random random, int topCount, int bottomCount)
        {
            for (int netIndex = topCount; netIndex < Pigeons.Length - bottomCount; netIndex++)
            {
                NeuralNetwork goodNet = Pigeons[random.Next(0, topCount)].Network;
                NeuralNetwork badNet = Pigeons[netIndex].Network;

                for (int layerIndex = 1; layerIndex < badNet.Layers.Length; layerIndex++)
                {
                    Layer goodLayer = goodNet.Layers[layerIndex];
                    Layer badLayer = badNet.Layers[layerIndex];

                    for (int neuronIndex = random.Next(0, badLayer.Neurons.Length); neuronIndex < badLayer.Neurons.Length; neuronIndex++)
                    {
                        Neuron goodNeuron = goodLayer.Neurons[neuronIndex];
                        Neuron badNeuron = badLayer.Neurons[neuronIndex];

                        badNeuron.Bias = goodNeuron.Bias;

                        for (int dendriteIndex = 0; dendriteIndex < badNeuron.Dendrites.Length; dendriteIndex++)
                        {
                            badNeuron.Dendrites[dendriteIndex].Weight = goodNeuron.Dendrites[dendriteIndex].Weight;
                        }
                    }
                }
            }
        }

        public void Train()
        {
            Array.Sort(Pigeons, comparer);
            if (bestFitness < Pigeons[0].Score)
            {
                bestFitness = Pigeons[0].Score;
            }
            int topCount = (int)(Pigeons.Length * TopSurvivalThreshold);
            int bottomCount = (int)(Pigeons.Length * BottomSurvivalThreshold);

            Crossover(Random, topCount, bottomCount);

            for (int i = topCount; i < Pigeons.Length - bottomCount; i++)
            {
                Pigeons[i].IsDead = false;
                Mutator.Mutate(Pigeons[i].Network, Random, MutationRate);
            }

            for (int i = Pigeons.Length - bottomCount; i < Pigeons.Length; i++)
            {
                Pigeons[i].Network.Randomize(Random, min, max);
            }
        }
    }
}