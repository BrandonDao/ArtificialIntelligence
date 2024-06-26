using SharedLibrary.States;

namespace SharedLibrary.Movement.Results
{
    public readonly struct PlanningResult<TState>(TState successor, StateToken<IState> successorToken, float cost, float probability)
            : IResult<TState>
        where TState : IState
    {
        public TState SuccessorState { get; } = successor;
        public StateToken<IState> SuccessorStateToken { get; } = successorToken;
        public float Cost { get; } = cost;
        public float Probability { get; } = probability;
    }
}