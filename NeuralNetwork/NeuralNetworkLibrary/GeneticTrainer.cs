using NeuralNetworkLibrary.NetworkStructure;

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

        public GeneticTrainer(Random random, int networkAmount, int[] neuronsPerLayer, double min, double max,
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

            MutationRate = 1;

            this.fitnessFunction = fitnessFunction;
            comparer = new NeuralNetworkComparer();
        }

        // layer crossover
        public void Crossover(Random random, int topCount, int bottomCount)
        {
            for (int l = topCount; l < Networks.Length - bottomCount; l++)
            {
                int layerIndex = random.Next(1, Networks[l].Layers.Length);

                Layer badLayer = Networks[l].Layers[layerIndex];
                Layer goodLayer = Networks[random.Next(0, topCount)].Layers[layerIndex];

                for (int n = 0; n < badLayer.Neurons.Length; n++)
                {
                    badLayer.Neurons[n].Bias = goodLayer.Neurons[n].Bias;

                    for (int d = 0; d < badLayer.Neurons[n].Dendrites.Length; d++)
                    {
                        badLayer.Neurons[n].Dendrites[d].Weight = goodLayer.Neurons[n].Dendrites[d].Weight;
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

            int topCount = (int)(Networks.Length * TopSurvivalThreshold);
            int bottomCount = (int)(Networks.Length * BottomSurvivalThreshold);

            Crossover(Random, topCount, bottomCount);

            for (int i = topCount; i < Networks.Length - bottomCount; i++)
            {
                Mutator.Mutate(Networks[i], Random, MutationRate);
            }

            for (int i = bottomCount; i < Networks.Length; i++)
            {
                Networks[i].Randomize(Random, min, max);
            }
        }
    }
}