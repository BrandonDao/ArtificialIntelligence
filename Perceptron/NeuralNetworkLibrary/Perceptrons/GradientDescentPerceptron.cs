namespace NeuralNetworkLibrary.Perceptrons
{
    public class GradientDescentPerceptron : Perceptron
    {
        public GradientDescentPerceptron(int amountOfInputs, double learningRate,
            ActivationFunction activationFunction, ErrorFunction errorFunc)
            : base(amountOfInputs, learningRate, activationFunction, errorFunc)
        {
            this.errorFunc = errorFunc;
        }

        public override double Compute(double[] inputs)
            => activationFunction.Function(base.Compute(inputs));

        public double Train(double[] inputs, double desiredOutput)
        {
            double output = base.Compute(inputs);
            double activatedOutput = activationFunction.Function(output);
            double error = errorFunc.Function(activatedOutput, desiredOutput);

            double weightChangeScalar = errorFunc.Derivative(activatedOutput, desiredOutput)
                * activationFunction.Derivative(output)
                * -LearningRate;

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] += weightChangeScalar * inputs[i];
            }
            bias += weightChangeScalar;

            return error;
        }

        public double Train(double[][] inputs, double[] desiredOutputs)
        {
            var outputs = new double[desiredOutputs.Length];
            var activatedOutputs = new double[desiredOutputs.Length];

            for (int i = 0; i < desiredOutputs.Length; i++)
            {
                outputs[i] = base.Compute(inputs[i]);
                activatedOutputs[i] = activationFunction.Function(outputs[i]);
            }

            double error = GetError(inputs, desiredOutputs);

            for (int i = 0; i < desiredOutputs.Length; i++)
            {
                double errorDeriv = errorFunc.Derivative(activatedOutputs[i], desiredOutputs[i]);

                double adjustmentVal = errorDeriv
                * activationFunction.Derivative(outputs[i])
                * -LearningRate;

                for (int j = 0; j < weights.Length; j++)
                {
                    weights[j] += adjustmentVal * inputs[i][j];
                }
                bias += adjustmentVal;

            }

            return error;
        }

        public void TrainFor(double[][] inputs, double[] desiredOutputs, int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                Train(inputs, desiredOutputs);
            }
        }
    }
}
