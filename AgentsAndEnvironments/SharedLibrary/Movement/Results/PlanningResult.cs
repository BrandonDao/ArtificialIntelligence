using SharedLibrary.States;

namespace SharedLibrary.Movement.Results
{
    public readonly struct PlanningResult<TState>(TState successor, StateToken<IState> successorToken, float cost, float probability)
            : IResult<TState>
        where TState : IState
    {
        public TState State { get; } = successor;
        public StateToken<IState> StateToken { get; } = successorToken;
        public float Cost { get; } = cost;
        public float Probability { get; } = probability;
    }
}