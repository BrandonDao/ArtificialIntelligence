namespace NeuralNetworkLibrary
{
    public class ActivationFunction
    {
        readonly Func<double, double> function;
        readonly Func<double, double> derivative;

        public ActivationFunction(Func<double, double> function, Func<double, double> derivative)
        {
            this.function = function;
            this.derivative = derivative;
        }

        public double Function(double input) => function(input);
        public double Derivative(double input) => derivative(input);

        public static ActivationFunction BinaryStep => new(
            function: (double x) => x < .5 ? 0 : 1,
            derivative: (double x) => 0);

        public static ActivationFunction Identity => new (
                function: (double x) => x,
                derivative: (double x) => 1);

        public static ActivationFunction Sigmoid => new(
            function: SigmoidFunc,
            derivative: (double x) => SigmoidFunc(x) * SigmoidFunc(1 - x));

        public static ActivationFunction TanH => new(
            function: TanHFunc,
            derivative: (double x) => 1 - Math.Pow(TanHFunc(x), 2));

        public static ActivationFunction ReLU => new(
            function: (double x) => x < 0 ? 0 : x,
            derivative: (double x) => x < 0 ? 0 : 1);

        private static double SigmoidFunc(double x) => 1 / (1 + Math.Pow(Math.E, -x));
        private static double TanHFunc(double x) => Math.Tanh(x);
    }
}