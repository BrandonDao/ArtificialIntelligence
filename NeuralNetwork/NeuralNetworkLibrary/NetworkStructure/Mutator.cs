namespace NeuralNetworkLibrary.NetworkStructure
{
    public class Mutator
    {
        public List<Action<Neuron, Random, double>> PossibleMutations { get; set; }

        public Mutator()
        {
            PossibleMutations = new List<Action<Neuron, Random, double>>();
        }

        public static void PercentChange(Neuron neuron, Random random, double mutationRate)
        {
            if (random.NextDouble() < mutationRate)
            {
                neuron.Bias *= random.NextDouble(0.5d, 1.5d);
            }

            for (int i = 0; i < neuron.Dendrites.Length; i++)
            {
                if (random.NextDouble() < mutationRate) continue;

                neuron.Dendrites[i].Weight *= random.NextDouble(0.5d, 1.5d);
            }
        }
        public static void FlipSign(Neuron neuron, Random random, double mutationRate)
        {
            if (random.NextDouble() < mutationRate)
            {
                neuron.Bias *= -1;
            }

            for (int i = 0; i < neuron.Dendrites.Length; i++)
            {
                if (random.NextDouble() < mutationRate) continue;

                neuron.Dendrites[i].Weight *= -1;
            }
        }

        public void Mutate(NeuralNetwork net, Random random, double mutationRate)
        {
            foreach (Layer layer in net.Layers)
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    if (random.NextDouble() < mutationRate)
                    {
                        var mutator = PossibleMutations[random.Next(0, PossibleMutations.Count)];

                        mutator.Invoke(neuron, random, mutationRate);
                    }

                    for (int i = 0; i < neuron.Dendrites.Length; i++)
                    {
                        if (random.NextDouble() >= mutationRate) continue;

                        var mutator = PossibleMutations[random.Next(0, PossibleMutations.Count)];

                        mutator.Invoke(neuron, random, mutationRate);
                    }
                }
            }

        }
    }
}