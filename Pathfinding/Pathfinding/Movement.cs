using Pathfinding.States;

namespace Pathfinding
{
    public readonly struct Movement<TState>(Movement<TState>.Result[] results)
        where TState : IState
    {
        public readonly struct Result(TState successor, StateToken<IState> successorToken, float cost, float probability)
        {
            public TState SuccessorState { get; } = successor;
            public StateToken<IState> SuccessorStateToken { get; } = successorToken;
            public float Cost { get; } = cost;
            public float Probability { get; } = probability;
        }

        public Result[] Results { get; } = results;
    }
}