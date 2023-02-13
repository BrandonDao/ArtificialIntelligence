namespace Perceptron
{
    public class Perceptron
    {
        private const double defaultMinValue = -100;
        private const double defaultMaxValue = 100;
        private double[] weights;
        private double bias;

        public Perceptron(double[] initialWeights, double initialBias)
        {
            weights = initialWeights;
            bias = initialBias;
        }

        public Perceptron(int amountOfInputs)
        {
            weights = new double[amountOfInputs];
            Randomize(new Random(), defaultMinValue, defaultMaxValue);
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
                throw new ArgumentOutOfRangeException(nameof(inputs), "The inputs.Length does not match the weights.Length!");

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

            for(int i = 0; i < outputs.Length; i++)
            {
                outputs[i] = Compute(inputs[i]);
            }

            return outputs;
        }
    }
}