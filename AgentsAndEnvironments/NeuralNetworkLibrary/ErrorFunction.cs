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

        public static ErrorFunction MeanSquaredError => new(
            function: (double actual, double expected) => Math.Pow(actual - expected, 2),
            derivative: (double actual, double expected) => 2 * (actual - expected));

        public static ErrorFunction MeanAbsoluteError = new(
            function: (double actual, double expected) => Math.Abs(actual - expected),
            derivative: (double actual, double expected) => 1);
    }
}