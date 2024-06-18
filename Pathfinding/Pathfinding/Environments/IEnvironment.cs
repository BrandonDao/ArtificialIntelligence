using Pathfinding.States;

namespace Pathfinding.Environments
{
    public interface IEnvironment<TState> where TState : IState
    {
        public TState GoalState { get; }
        public List<Edge<TState>> GetSuccessors(IState state);
        public Agent<TState>.AgentData MakeMove(Agent<TState>.AgentData newState);
    }
}