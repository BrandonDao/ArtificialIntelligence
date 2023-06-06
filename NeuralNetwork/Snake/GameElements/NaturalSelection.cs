using NeuralNetworkLibrary.NetworkStructure;
using System;
using System.Collections.Generic;

namespace Snake.NetworkElements
{
    public class HabitatComparer : IComparer<Habitat>
    {
        public int Compare(Habitat x, Habitat y)
            => x.Python.Score > y.Python.Score ? -1 : x.Python.Score < y.Python.Score ? 1 : 0;
    }

    public class NaturalSelection
    {
        public Random Random { get; set; }

        public const double TopSurvivalThreshold = .10d;
        public const double BottomSurvivalThreshold = .10d;

        public double MutationRate { get; set; }

        public Mutator Mutator { get; set; }
        public Habitat[] Habitats;


        private readonly HabitatComparer comparer;

        private readonly double min;
        private readonly double max;

        public NaturalSelection(Random random, Mutator mutator, Habitat[] habitats, double mutationRate, double min, double max)
        {
            Random = random;
            Mutator = mutator;
            Habitats = habitats;
            MutationRate = mutationRate;
            this.min = min;
            this.max = max;
        }

        public NaturalSelection(Random random, Habitat[] habitats, double mutationRate, double min, double max)
        {
            Random = random;

            Habitats = habitats;
            foreach (var habitat in Habitats)
            {
                habitat.Python.Network.Randomize(random, min, max);
            }

            this.min = min;
            this.max = max;

            Mutator = new Mutator();
            Mutator.PossibleMutations.Add(Mutator.PercentChange);
            Mutator.PossibleMutations.Add(Mutator.FlipSign);

            MutationRate = mutationRate;

            comparer = new HabitatComparer();
        }

        // layer-style crossover
        private void Crossover(Random random, int topCount, int bottomCount)
        {
            for (int netIndex = topCount; netIndex < Habitats.Length - bottomCount; netIndex++)
            {
                NeuralNetwork goodNet = Habitats[random.Next(0, topCount)].Python.Network;
                NeuralNetwork badNet = Habitats[netIndex].Python.Network;

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

        public void Select()
        {
            Array.Sort(Habitats, comparer);
            int topCount = (int)(Habitats.Length * TopSurvivalThreshold);
            int bottomCount = (int)(Habitats.Length * BottomSurvivalThreshold);

            Crossover(Random, topCount, bottomCount);

            for (int i = topCount; i < Habitats.Length - bottomCount; i++)
            {
                Habitats[i].Python.IsDead = false;
                Mutator.Mutate(Habitats[i].Python.Network, Random, MutationRate);
            }

            for (int i = Habitats.Length - bottomCount; i < Habitats.Length; i++)
            {
                Habitats[i].Python.Network.Randomize(Random, min, max);
            }
        }
    }
}