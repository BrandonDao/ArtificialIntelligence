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
    }
}