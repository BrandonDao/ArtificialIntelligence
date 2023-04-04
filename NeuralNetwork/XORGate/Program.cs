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
            double[] desiredOutputs = { 0, 1, 1, 0 };

            var outputs = new double[4];

            double fitnessFunc(NeuralNetwork net)
            {
                double error = 0;
                for (int i = 0; i < outputs.Length; i++)
                {
                    var output = net.Compute(inputs[i])[0];
                    error += net.GetError(outputs, desiredOutputs);
                }

                double fitness = 1 / (error == 0 ? 1 : error);

                Console.WriteLine($"fitness: {fitness}");

                return fitness;
            }

            Random random = new('c' + 'a' + 't');

            var trainer = new GeneticTrainer(random, networkAmount: 3, neuronsPerLayer: new int[] { 2, 2, 1 },
                min: 0, max: 1, ActivationFunction.Identity, ErrorFunction.MeanSquaredError, fitnessFunc);

            while(true)
            {
                trainer.Train();
            }
        }
    }
}