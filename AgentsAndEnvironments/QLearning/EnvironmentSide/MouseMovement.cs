namespace QLearning.EnvironmentSide
{
    public readonly struct MouseMovement(MouseResult[] results)
    {
        public readonly MouseResult[] Results = results;
    }
}