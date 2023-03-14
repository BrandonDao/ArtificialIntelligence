using NeuralNetworkLibrary;
using NeuralNetworkLibrary.Perceptrons;

namespace LogicGates
{
    public class Program
    {
        private static ErrorFunction ErrorFunc = new(ErrorFunction.MeanAbsoluteError, ErrorFunction.MeanAbsoluteErrorDerivative);

        static void Main(string[] args)
        {
            Random random = new('c'+'a'+'t');

            ActivationFunction actFunc = new(ActivationFunction.Identity, ActivationFunction.IdentityDerivative);
            GradientDescentPerceptron AndPerceptron = new(random, amountOfInputs: 2, learningRate: 0.0001d, actFunc, ErrorFunc);
            GradientDescentPerceptron OrPerceptron = new(random, amountOfInputs: 2, learningRate: 0.0001d, actFunc, ErrorFunc);

            var inputs = new double[][]
            {
                new double[] { 0, 0 },
                new double[] { 0, 1 },
                new double[] { 1, 0 },
                new double[] { 1, 1 },
            };
            var AndOutputs = new double[] { 0, 0, 0, 1 };
            var OrOutputs = new double[] { 0, 1, 1, 1 };

            double AndError = AndPerceptron.GetError(inputs, AndOutputs);
            double OrError = OrPerceptron.GetError(inputs, OrOutputs);

            while (true)
            {
                AndError = AndPerceptron.Train(inputs, AndOutputs);
                OrError = OrPerceptron.Train(inputs, OrOutputs);

                Console.Clear();
                Console.WriteLine("AND Gate\n  in    out  binStep  sigmoid  tanH  ReLU  Rounded");

                for (int i = 0; i < inputs.Length; i++)
                {
                    double[] input = inputs[i];

                    double rawOutput = AndPerceptron.Compute(input);
                    double output = Math.Round(rawOutput, digits: 1);
                    double binOutput = Math.Round(ActivationFunction.BinaryStep(rawOutput), digits: 1);
                    double sigOutput = Math.Round(ActivationFunction.Sigmoid(rawOutput), digits: 1);
                    double tanHOutput = Math.Round(ActivationFunction.Sigmoid(rawOutput), digits: 1);
                    double reLUOutput = Math.Round(ActivationFunction.ReLU(rawOutput), digits: 1);
                    double rounded = (int)Math.Round(rawOutput);

                    double expectedOutput = AndOutputs[i];

                    Console.WriteLine($"{(int)input[0]} & {(int)input[1]}: {output, 4}{binOutput, 5}" +
                        $"{sigOutput, 11}{tanHOutput,7}{reLUOutput,6}{rounded,7}");
                }


                Console.WriteLine("\nOR Gate");

                for (int i = 0; i < inputs.Length; i++)
                {
                    double[] input = inputs[i];

                    double rawOutput = OrPerceptron.Compute(input);
                    double output = Math.Round(rawOutput, digits: 1);
                    double binOutput = Math.Round(ActivationFunction.BinaryStep(rawOutput), digits: 1);
                    double sigOutput = Math.Round(ActivationFunction.Sigmoid(rawOutput), digits: 1);
                    double tanHOutput = Math.Round(ActivationFunction.Sigmoid(rawOutput), digits: 1);
                    double reLUOutput = Math.Round(ActivationFunction.ReLU(rawOutput), digits: 1);
                    double rounded = (int)Math.Round(rawOutput);

                    double expectedOutput = AndOutputs[i];

                    Console.WriteLine($"{(int)input[0]} & {(int)input[1]}: {output,4}{binOutput,5}" +
                        $"{sigOutput,11}{tanHOutput,7}{reLUOutput,6}{rounded,7}");
                }

                Thread.Sleep(1);
            }

        }
    }
}