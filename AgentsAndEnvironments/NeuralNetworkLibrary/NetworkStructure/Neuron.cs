namespace NeuralNetworkLibrary.NetworkStructure
{
    public class Neuron
    {
        public ActivationFunction ActivationFunc { get; set; }
        public double ActivatedOutput { get; set; }
        public double RawOutput { get; set; }

        public double Bias { get; set; }
        public Dendrite[] Dendrites { get; set; }

        public double Delta { get; set; }
        private double biasUpdate;

        public Neuron(ActivationFunction activationFunc, Neuron[] previousNeurons)
        {
            ActivationFunc = activationFunc;

            Dendrites = new Dendrite[previousNeurons.Length];
            for (int i = 0; i < previousNeurons.Length; i++)
            {
                Dendrites[i] = new Dendrite(previousNeurons[i], next: this, weight: 0);
            }
        }

        public Neuron(ActivationFunction activationFunc)
        {
            ActivationFunc = activationFunc;
            Dendrites = Array.Empty<Dendrite>();
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
            RawOutput = Bias + Dendrites.Sum((dendrite) => dendrite.Previous.ActivatedOutput * dendrite.Weight);
            ActivatedOutput = ActivationFunc.Function(RawOutput);

            return ActivatedOutput;
        }

        public void Backprop(double learningRate)
        {
            double activationFuncDerivative = ActivationFunc.Derivative(RawOutput);

            double a = learningRate * (Delta * activationFuncDerivative);
            biasUpdate -= a;

            foreach (var dendrite in Dendrites)
            {
                dendrite.Previous.Delta += Delta * activationFuncDerivative * dendrite.Weight;

                dendrite.WeightUpdate -= a * dendrite.Previous.ActivatedOutput;
            }
            Delta = 0;
        }

        public void ApplyUpdates()
        {
            Bias += biasUpdate;

            foreach (var dendrite in Dendrites)
            {
                dendrite.Weight += dendrite.WeightUpdate;
                dendrite.WeightUpdate = 0;
            }
            Delta = 0;
            biasUpdate = 0;
        }
    }
}