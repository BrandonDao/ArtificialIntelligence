using System;

namespace LineOfBestFitVisualizer
{
    public class Perceptron
    {
        private const double DefaultMinValue = -10;
        private const double DefaultMaxValue = 10;
        private const double DefaultMutationAmount = 1;

        private Random random;
        private double[] weights;
        private double bias;
        double mutationAmount;
        Func<double, double, double> errorFunc;

        public Perceptron(Random random, double[] initialWeights, double initialBias, double mutationAmount, Func<double, double, double> errorFunc)
        {
            weights = initialWeights;
            bias = initialBias;
            this.random = random;
            this.mutationAmount = mutationAmount;
            this.errorFunc = errorFunc;
        }

        public Perceptron(Random random, int amountOfInputs, double initialBias, Func<double, double, double> errorFunc)
        {
            weights = new double[amountOfInputs];
            mutationAmount = DefaultMutationAmount;
            this.random = random;
            this.errorFunc = errorFunc;

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

        public double GetError(double[][] inputs, double[] desiredOutputs)
        {
            double sum = 0;

            double[] outputs = Compute(inputs);

            for(int i = 0; i < outputs.Length; i++)
            {
                sum += errorFunc.Invoke(outputs[i], desiredOutputs[i]);
            }
            return sum / desiredOutputs.Length;
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

        public double TrainWithHillClimbing(double[][] inputs, double[] desiredOutputs, double currentError)
        {
            double sum = 0;

            for (int i = 0; i < inputs.GetLength(0); i++)
            {
                var savedWeights = new double[inputs[i].Length];
                weights.CopyTo(array: savedWeights, index: 0);

                int mutationIndex = random.Next(0, weights.Length + 1);
                double mutationValue = random.NextDouble() * (-2 * mutationAmount) + mutationAmount;

                if (mutationIndex == weights.Length)
                {
                    bias += mutationValue;
                }
                else
                {
                    weights[mutationIndex] += mutationValue;
                }

                double newError = GetError(inputs, desiredOutputs);

                if (newError >= currentError)
                {
                    inputs[i] = savedWeights;
                    newError = currentError;
                }

                sum += newError;
            }

            return sum / inputs.GetLength(0);
        }
    }
}