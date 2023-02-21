namespace Perceptron
{
    public class Perceptron
    {
        protected double[] weights;
        protected double bias;
        protected Func<double, double, double> errorFunc;

        public Perceptron(double[] initialWeights, double initialBias, double mutationAmount, Func<double, double, double> errorFunc)
        {
            weights = initialWeights;
            bias = initialBias;
            this.errorFunc = errorFunc;
        }

        public Perceptron(int amountOfInputs, double initialBias, Func<double, double, double> errorFunc)
        {
            weights = new double[amountOfInputs];
            this.errorFunc = errorFunc;
        }

        public void Randomize(Random random, double min, double max)
        {
            bias = random.NextDouble() * (max - min) + min;

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = random.NextDouble() * (max - min) + min;
            }
        }

        public double GetError(double[][] inputs, double[] desiredOutputs)
        {
            double sum = 0;

            double[] outputs = Compute(inputs);

            for (int i = 0; i < outputs.Length; i++)
            {
                sum += errorFunc.Invoke(outputs[i], desiredOutputs[i]);
            }
            return sum / desiredOutputs.Length;
        }

        public double Compute(double[] inputs)
        {
            if (inputs.Length != weights.Length)
            {
                string name = nameof(inputs);
                throw new ArgumentOutOfRangeException(name, $"The number of elements in {name} does not match the number of elements in weights!");
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