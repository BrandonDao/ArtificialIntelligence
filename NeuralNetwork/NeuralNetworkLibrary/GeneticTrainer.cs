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

        public double TopSurvivalThreshold { get; set; } = 10;
        public double BottomSurvivalThreshold { get; set; } = 10;

        public double MutationRate { get; set; }

        public Mutator Mutator { get; set; }

        List<NeuralNetwork> networks;
        NeuralNetworkComparer comparer;

        public GeneticTrainer(Random random, int min, int max, int networkAmount, Func<NeuralNetwork, double> fitnessFunction)
        {
            Random = random;

            networks = new List<NeuralNetwork>(networkAmount);
            networks.ForEach((NeuralNetwork n) => n.Randomize(Random, min, max));

            Mutator = new Mutator();

            MutationRate = 1;

            comparer = new NeuralNetworkComparer(fitnessFunction);
        }

        // layer crossover
        public void Crossover(NeuralNetwork net, Random random)
        {
            int mid = net.Layers.Length / 2;

            var layerA = net.Layers[random.Next(0, mid)];
            var layerB = net.Layers[random.Next(mid, net.Layers.Length)];

            var temp = new Layer();
        }

        public void Train()
        {
            networks.Sort(comparer);

            /*
             * if (first == good)
             * {
             *     finish training
             * }
             */

            int topCount = (int)(networks.Count * TopSurvivalThreshold);
            int bottomCount = (int)(networks.Count * BottomSurvivalThreshold);

            for (int i = topCount; i < networks.Count - bottomCount; i++)
            {
                // crossover
            }

            for (int i = topCount; i < networks.Count - bottomCount; i++)
            {
                Mutator.Mutate(networks[i], Random, MutationRate);
            }
        }



    }
}