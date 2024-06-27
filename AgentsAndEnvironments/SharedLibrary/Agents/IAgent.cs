using SharedLibrary.States;

namespace SharedLibrary.Agents
{
    public interface IAgent<TState>
        where TState : IState
    {
        public bool MakeMove(Predicate<TState> hasReachedGoal);
    }
}
