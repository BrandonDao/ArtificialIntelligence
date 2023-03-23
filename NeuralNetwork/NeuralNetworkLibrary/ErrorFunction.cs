namespace NeuralNetworkLibrary
{
    public class ErrorFunction
    {
        Func<double, double, double> function;
        Func<double, double, double> derivative;

        public ErrorFunction(Func<double, double, double> function, Func<double, double, double> derivative)
        {
            this.function = function;
            this.derivative = derivative;
        }

        public double Function(double output, double desiredOutput) => function(output, desiredOutput);
        public double Derivative(double output, double desiredOutput) => derivative(output, desiredOutput);

        public static double MeanSquaredError(double actual, double expected) => Math.Pow(actual - expected, 2);
        public static double MeanSquaredErrorDerivative(double actual, double expected) => 2 * (actual - expected);

        public static double MeanAbsoluteError(double actual, double expected) => Math.Abs(actual - expected);
        public static double MeanAbsoluteErrorDerivative(double actual, double expected) => 1;
    }
}