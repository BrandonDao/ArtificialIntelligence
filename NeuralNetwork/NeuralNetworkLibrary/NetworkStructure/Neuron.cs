namespace NeuralNetworkLibrary.NetworkStructure
{
    public class Neuron
    {
        public ActivationFunction ActivationFunc { get; set; }
        public double ActivatedOutput { get; set; }
        public double RawOutput { get; set; }

        public double Bias { get; set; }
        public Dendrite[] Dendrites { get; set; }

        public Neuron(ActivationFunction activationFunc, Neuron[] previousNerons)
        {
            ActivationFunc = activationFunc;

            Dendrites = new Dendrite[previousNerons.Length];
            for (int i = 0; i < previousNerons.Length; i++)
            {
                Dendrites[i] = new Dendrite(previousNerons[i], next: this, weight: 0);
            }
        }

        public void Randomize(Random random, double min, double max)
        {
            Bias = random.NextDouble(min, max);

            for (int i = 0; i < Dendrites.Length; i++)
            {
                Dendrites[i].Weight = random.NextDouble(min, max);
            }
        }

        public double Compute()
        {
            RawOutput = Bias + Dendrites.Sum((dendrite) => dendrite.Previous.ActivatedOutput);
            ActivatedOutput = ActivationFunc.Function(RawOutput);

            return ActivatedOutput;
        }
    }
}