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
                for (int i = 0; i < outputs.Length; i++)
                {
                    outputs[i] = net.Compute(inputs[i])[0];
                }
                return -net.GetError(outputs, desiredOutputs);
            }

            Random random = new();

            var trainer = new GeneticTrainer(random, networkAmount: 100, neuronsPerLayer: new int[] { 2, 2, 1 },
                min: -1, max: 1, mutationRate: .5f, ActivationFunction.TanH, ErrorFunction.MeanSquaredError, fitnessFunc);


            while(true)
            {
                trainer.Train();

                var top = trainer.Networks[0];
                Console.Clear();
                for (int i = 0; i < outputs.Length; i++)
                {
                    outputs[i] = top.Compute(inputs[i])[0];
                }

                for (int i = 0; i < outputs.Length; i++)
                {
                    Console.WriteLine($"{inputs[i][0]} xor {inputs[i][1]}: {Math.Round(outputs[i], 2)}");
                    Console.WriteLine($"error:   {Math.Round(trainer.Networks[0].GetError(new double[] { outputs[i] }, new double[] { desiredOutputs[i] }), 2)}\n");
                }

                Thread.Sleep(1);
            }
        }
    }
}