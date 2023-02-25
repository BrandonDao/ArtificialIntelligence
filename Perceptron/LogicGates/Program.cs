using Perceptron;

namespace LogicGates
{
    public class Program
    {
        private static double ErrorFunc(double actual, double expected) => Math.Abs(actual - expected);

        static void Main(string[] args)
        {
            Random random = new('c'+'a'+'t');

            HillClimbingPerceptron AndPerceptron = new(random, amountOfInputs: 2, initialBias: 0, mutationAmount: .25d, ErrorFunc);
            HillClimbingPerceptron OrPerceptron = new(random, amountOfInputs: 2, initialBias: 0, mutationAmount: .25d, ErrorFunc);

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
                AndError = AndPerceptron.Train(inputs, AndOutputs, AndError);
                OrError = OrPerceptron.Train(inputs, OrOutputs, OrError);

                Console.Clear();
                Console.WriteLine("AND Gate\n  in    out   rounded");

                for (int i = 0; i < inputs.Length; i++)
                {
                    double[] input = inputs[i];
                    double output = Math.Round(AndPerceptron.Compute(input), digits: 1);
                    int roundedOutput = (int)Math.Round(output);
                    double expectedOutput = AndOutputs[i];

                    Console.Write($"{(int)input[0]} & {(int)input[1]}: {output, 4} ");
                    Console.ForegroundColor = roundedOutput == expectedOutput ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.WriteLine($"    {roundedOutput,2}");
                    Console.ForegroundColor = ConsoleColor.White;
                }


                Console.WriteLine("\nOR Gate");

                for (int i = 0; i < inputs.Length; i++)
                {
                    double[] input = inputs[i];
                    double output = Math.Round(OrPerceptron.Compute(input), digits: 1);
                    int roundedOutput = (int)Math.Round(output);
                    double expectedOutput = OrOutputs[i];

                    Console.Write($"{(int)input[0]} & {(int)input[1]}: {output,4} ");
                    Console.ForegroundColor = roundedOutput == expectedOutput ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.WriteLine($"    {roundedOutput,2}");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Thread.Sleep(1);
            }

        }
    }
}