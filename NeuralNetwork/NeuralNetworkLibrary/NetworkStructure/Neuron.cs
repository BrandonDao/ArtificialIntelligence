namespace NeuralNetworkLibrary.NetworkStructure
{
    public class Neuron
    {
        public double ActivatedOutput { get; set; }
        public double RawOutput { get; private set; }
        public ActivationFunction ActivationFunc { get; set; }

        double bias;
        Dendrite[] dendrites;

        public Neuron(ActivationFunction activationFunc, Neuron[] previousNerons)
        {
            ActivationFunc = activationFunc;

            dendrites = new Dendrite[previousNerons.Length];
            for (int i = 0; i < previousNerons.Length; i++)
            {
                dendrites[i] = new Dendrite(previousNerons[i], next: this, weight: 0);
            }
        }

        public void Randomize(Random random, double min, double max)
        {
            bias = random.NextDouble(min, max);

            for (int i = 0; i < dendrites.Length; i++)
            {
                dendrites[i].Weight = random.NextDouble(min, max);
            }
        }

        public double Compute()
        {
            RawOutput = bias + dendrites.Sum((dendrite) => dendrite.Previous.ActivatedOutput);
            ActivatedOutput = ActivationFunc.Function(RawOutput);

            return ActivatedOutput;
        }
    }
}
