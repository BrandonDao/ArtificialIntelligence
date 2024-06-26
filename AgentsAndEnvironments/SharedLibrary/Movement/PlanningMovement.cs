using SharedLibrary.Movement.Results;
using SharedLibrary.States;

namespace SharedLibrary.Movement
{
    public readonly struct PlanningMovement<TState>(PlanningResult<TState>[] results) : IMovement<TState, PlanningResult<TState>>
        where TState : IState
    {
        public PlanningResult<TState>[] Results { get; } = results;
    }
}