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


        public static double Identity(double x) => x;
        public static double IdentityDerivative(double x) => 1;

        public static double BinaryStep(double x) => x < 0 ? 0 : 1;

        public static double Sigmoid(double x) => 1 / (1 + Math.Pow(Math.E, -x));
        public static double SigmoidDerivative(double x) => Sigmoid(x) * Sigmoid(1 - x);

        public static double TanH(double x) => Math.Tanh(x);
        public static double TanHDerivative(double x) => 1 - Math.Pow(TanH(x), 2);

        public static double ReLU(double x) => x < 0 ? 0 : x;
        public static double ReLUDerivative(double x) => x < 0 ? 0 : 1;
    }
}