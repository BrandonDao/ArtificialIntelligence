namespace Perceptron
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var test = new Perceptron(new Random(42), new double[] { .75, -1.25 }, .5);
            var outputs = test.Compute(new double[][]
            {
                new double[] { 0, 0 },
                new double[] { 0.3, -0.7 },
                new double[] { 1, 1 },
                new double[] { -1, -1 },
                new double[] { -0.5, 0.5 } });

            ;
        }
    }
}