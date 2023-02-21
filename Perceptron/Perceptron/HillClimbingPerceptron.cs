namespace Perceptron
{
    public class HillClimbingPerceptron : Perceptron
    {
        private double mutationAmount;
        private Random random;

        public HillClimbingPerceptron(Random random, double[] initialWeights, double initialBias, double mutationAmount, Func<double, double, double> errorFunc)
            : base(initialWeights, initialBias, mutationAmount, errorFunc)
        {
            this.random = random;
            this.mutationAmount = mutationAmount;
        }

        public HillClimbingPerceptron(Random random, int amountOfInputs, double initialBias, double mutationAmount, Func<double, double, double> errorFunc)
            : base(amountOfInputs, initialBias, errorFunc)
        {
            this.random = random;
            this.mutationAmount = mutationAmount;
            this.errorFunc = errorFunc;
        }

        public double Train(double[][] inputs, double[] desiredOutputs, double currentError)
        {
            double sum = 0;

            for (int i = 0; i < inputs.GetLength(0); i++)
            {
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
                    if (mutationIndex == weights.Length)
                    {
                        bias -= mutationValue;
                    }
                    else
                    {
                        weights[mutationIndex] -= mutationValue;
                    }
                }

                sum += newError;
            }

            return sum / inputs.GetLength(0);
        }
    }
}