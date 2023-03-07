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
    }
}