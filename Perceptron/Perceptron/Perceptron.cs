namespace Perceptron
{
    public class Perceptron
    {
        private const double DefaultMinValue = -100;
        private const double DefaultMaxValue = 100;

        private Random random;
        private double[] weights;
        private double bias;

        public Perceptron(Random random, double[] initialWeights, double initialBias)
        {
            weights = initialWeights;
            bias = initialBias;
            this.random = random;
        }

        public Perceptron(Random random, int amountOfInputs)
        {
            weights = new double[amountOfInputs];
            this.random = random;

            Randomize(random, DefaultMinValue, DefaultMaxValue);
        }

        public void Randomize(Random random, double min, double max)
        {
            bias = random.NextDouble() * (max - min) + min;

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = random.NextDouble() * (max - min) + min;
            }
        }

        public double Compute(double[] inputs)
        {
            if (inputs.Length != weights.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(inputs), "The inputs.Length does not match the weights.Length!");
            }

            double output = bias;
            for (int i = 0; i < inputs.Length; i++)
            {
                output += inputs[i] * weights[i];
            }
            return output;
        }

        public double[] Compute(double[][] inputs)
        {
            var outputs = new double[inputs.GetLength(0)];

            for (int i = 0; i < outputs.Length; i++)
            {
                outputs[i] = Compute(inputs[i]);
            }

            return outputs;
        }
    }
}