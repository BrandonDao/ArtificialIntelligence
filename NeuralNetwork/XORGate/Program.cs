using NeuralNetworkLibrary;
using NeuralNetworkLibrary.NetworkStructure;

namespace XORGate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            double[][] inputs = new double[][]
            {
                new double[] { 0, 0 },
                new double[] { 0, 1 },
                new double[] { 1, 0 },
                new double[] { 1, 1 }
            };
            double[] desiredOutputs = { 0, 0, 0, 0 };

            var outputs = new double[4];



            double fitnessFunc(NeuralNetwork net)
            {
                for (int i = 0; i < outputs.Length; i++)
                {
                    outputs[i] = net.Compute(inputs[i])[0];
                }
                return -net.GetError(outputs, desiredOutputs);
            }

            Random random = new(/*'c' + 'a' + 't'*/);

            var trainer = new GeneticTrainer(random, networkAmount: 100, neuronsPerLayer: new int[] { 2, 2, 1 },
                min: 0, max: 1, ActivationFunction.Sigmoid, ErrorFunction.MeanSquaredError, fitnessFunc);

            while(true)
            {
                trainer.Train();

                var top = trainer.Networks[0];
                Console.Clear();
                for (int i = 0; i < outputs.Length; i++)
                {
                    outputs[i] = top.Compute(inputs[i])[0];
                    Console.WriteLine($"{inputs[i][0]} xor {inputs[i][1]}: {outputs[i]}");
                }
                Thread.Sleep(1);
            }
        }
    }
}