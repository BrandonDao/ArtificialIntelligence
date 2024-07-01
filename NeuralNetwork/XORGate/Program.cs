using NeuralNetworkLibrary.NetworkStructure;
using NeuralNetworkLibrary;

namespace XORGate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var inputs = new double[][]
            {
                new double[] { 0, 0 },
                new double[] { 0, 1 },
                new double[] { 1, 0 },
                new double[] { 1, 1 }
            };
            var desiredOutputs = new double[][]
            {
                new double[] { 0 },
                new double[] { 1 },
                new double[] { 1 },
                new double[] { 0 }
            };

            var outputs = new double[4];

            //double fitnessFunc(NeuralNetwork net)
            //{
            //    for (int i = 0; i < outputs.Length; i++)
            //    {
            //        outputs[i] = net.Compute(inputs[i])[0];
            //    }
            //    return -net.GetError(outputs, desiredOutputs);
            //}

            //Random random = new();

            //var trainer = new GeneticTrainer(random, networkAmount: 100, neuronsPerLayer: new int[] { 2, 2, 1 },
            //    min: -1, max: 1, mutationRate: .5f, ActivationFunction.TanH, ErrorFunction.MeanSquaredError, fitnessFunc);

            var net = new NeuralNetwork(ActivationFunction.TanH, ErrorFunction.MeanSquaredError, neuronsPerLayer: new int[] { 2, 2, 1 });
            net.Randomize(Random.Shared, -1, 1);

            double prevError = 0;


            Console.WriteLine($@"    1 -> {Math.Round(net.Layers[1].Neurons[0].Dendrites[0].Weight, 2),5} -> (bias: {Math.Round(net.Layers[1].Neurons[0].Bias, 2),5}) -> {Math.Round(net.Layers[1].Neurons[0].ActivatedOutput, 2),5} \");
            Console.WriteLine($@"       \ {Math.Round(net.Layers[1].Neurons[0].Dendrites[1].Weight, 2),5}                             {Math.Round(net.Layers[2].Neurons[0].Dendrites[0].Weight, 2),5} \");
            Console.WriteLine($@"        X                                          (bias: {Math.Round(net.Layers[2].Neurons[0].Bias, 2),5}) -> {Math.Round(net.Layers[2].Neurons[0].ActivatedOutput, 2),5}");
            Console.WriteLine($@"       / {Math.Round(net.Layers[1].Neurons[1].Dendrites[0].Weight, 2),5}                             {Math.Round(net.Layers[2].Neurons[0].Dendrites[1].Weight, 2),5} /");
            Console.WriteLine($@"    1 -> {Math.Round(net.Layers[1].Neurons[1].Dendrites[1].Weight, 2),5} -> (bias {Math.Round(net.Layers[1].Neurons[1].Bias, 2),5}) -> {Math.Round(net.Layers[1].Neurons[1].ActivatedOutput, 2),5} /");



            while (true)
            {
                // Console.ReadKey();
                // trainer.Train();
                // var net = trainer.Networks[0];

                double error = net.TrainWithGradientDescent(inputs, desiredOutputs, learningRate: 0.075f);

                for (int i = 0; i < outputs.Length; i++)
                {
                    outputs[i] = net.Compute(inputs[i])[0];
                }
                Console.Clear();

                for (int i = 0; i < outputs.Length; i++)
                {
                    Console.WriteLine($"{inputs[i][0]} xor {inputs[i][1]}: {Math.Round(outputs[i], 2)}");
                    //Console.WriteLine($"error:   {Math.Round(trainer.Networks[0].GetError(new double[] { outputs[i] }, new double[] { desiredOutputs[i] }), 2)}\n");
                    Console.WriteLine($"error:   {Math.Round(net.GetError(new double[] { outputs[i] }, new double[] { desiredOutputs[i][0] }), 5)}\n");
                }

                Console.Write("\nNetwork Error: ");
                Console.ForegroundColor = error >= prevError ? ConsoleColor.Red : ConsoleColor.Green;
                Console.WriteLine(error);
                Console.ForegroundColor = ConsoleColor.White;

                prevError = error;

                Console.WriteLine("\n");

                Console.WriteLine($@"    1 -> {Math.Round(net.Layers[1].Neurons[0].Dendrites[0].Weight, 2),5} -> (bias: {Math.Round(net.Layers[1].Neurons[0].Bias, 2),5}) -> {Math.Round(net.Layers[1].Neurons[0].ActivatedOutput,2),5} \");
                Console.WriteLine($@"       \ {Math.Round(net.Layers[1].Neurons[0].Dendrites[1].Weight, 2),5}                             {Math.Round(net.Layers[2].Neurons[0].Dendrites[0].Weight, 2),5} \");
                Console.WriteLine($@"        X                                          (bias: {Math.Round(net.Layers[2].Neurons[0].Bias, 2),5}) -> {Math.Round(net.Layers[2].Neurons[0].ActivatedOutput, 2),5}");
                Console.WriteLine($@"       / {Math.Round(net.Layers[1].Neurons[1].Dendrites[0].Weight, 2),5}                             {Math.Round(net.Layers[2].Neurons[0].Dendrites[1].Weight, 2),5} /");
                Console.WriteLine($@"    1 -> {Math.Round(net.Layers[1].Neurons[1].Dendrites[1].Weight, 2),5} -> (bias {Math.Round(net.Layers[1].Neurons[1].Bias, 2),5}) -> {Math.Round(net.Layers[1].Neurons[1].ActivatedOutput,2),5} /");


                Thread.Sleep(1);
            }
        }
    }
}