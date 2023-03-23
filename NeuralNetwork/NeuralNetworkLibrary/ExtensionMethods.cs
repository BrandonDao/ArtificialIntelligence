namespace NeuralNetworkLibrary
{
    public static class ExtensionMethods
    {
        public static double NextDouble(this Random random, double min, double max)
            => random.NextDouble() * (max - min) + min;
    }
}
