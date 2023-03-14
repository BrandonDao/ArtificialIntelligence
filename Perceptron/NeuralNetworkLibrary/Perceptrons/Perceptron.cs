namespace NeuralNetworkLibrary.Perceptrons
{
    public class Perceptron
    {
        public double LearningRate { get; set; }

        protected double[] weights;
        protected double bias;
        protected ErrorFunction errorFunc;
        protected ActivationFunction activationFunction;

        public Perceptron(int amountOfInputs, double learningRate, ActivationFunction activationFunction, ErrorFunction errorFunc)
        {
            weights = new double[amountOfInputs];

            LearningRate = learningRate;
            this.errorFunc = errorFunc;
            this.activationFunction = activationFunction;
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

        public double GetError(double[] inputs, double desiredOutputs)
            => errorFunc.Function(Compute(inputs), desiredOutputs);

        public double GetError(double[][] inputs, double[] desiredOutputs)
        {
            double sum = 0;

            for (int i = 0; i < desiredOutputs.Length; i++)
            {
                sum += GetError(inputs[i], desiredOutputs[i]);
            }
            return sum / desiredOutputs.Length;
        }

        public virtual double Compute(double[] inputs)
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
        public virtual double[] Compute(double[][] inputs)
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