using Pathfinding.States;

namespace Pathfinding.Agents
{
    public interface IAgent<TState>
        where TState : IState
    {
        public bool MakeMove(Predicate<TState> hasReachedGoal);
        public AgentData<TState> GetFinishedState();
    }
}
