namespace NeuralNetworkLibrary.Perceptrons
{
    public class HillClimbingPerceptron : Perceptron
    {
        private readonly Random random;

        public HillClimbingPerceptron(Random random, int amountOfInputs, double learningRate,
            ActivationFunction activationFunction, ErrorFunction errorFunc)
            : base(amountOfInputs, learningRate, activationFunction, errorFunc)
        {
            this.random = random;
            this.errorFunc = errorFunc;
        }

        public double Train(double[] inputs, double desiredOutput, double currentError)
        {
            int mutationIndex = random.Next(0, weights.Length + 1);
            double mutationValue = random.NextDouble() * (-2 * LearningRate) + LearningRate;

            if (mutationIndex == weights.Length)
            {
                bias += mutationValue;
            }
            else
            {
                weights[mutationIndex] += mutationValue;
            }

            double newError = GetError(inputs, desiredOutput);

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
                newError = currentError;
            }

            return newError;
        }
        public double Train(double[][] inputs, double[] desiredOutputs, double currentError)
        {
            int mutationIndex = random.Next(0, weights.Length + 1);
            double mutationValue = random.NextDouble() * (-2 * LearningRate) + LearningRate;

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
                newError = currentError;
            }

            return newError;
        }
    }
}