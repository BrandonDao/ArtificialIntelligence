using Pathfinding.States;

namespace Pathfinding
{
    public class StateToken<TState>(TState state)
        where TState : IState
    {
        public TState State { get; } = state;
    }
}