using Pathfinding.Agents;
using Pathfinding.States;

namespace Pathfinding.Environments
{
    public interface IEnvironment<TState> where TState : IState
    {
        public TState GoalState { get; }
        public void RegisterAgent(StateToken<IState> currentStateToken, TState state);
        public List<Edge<TState>> GetSuccessors(StateToken<IState> stateToken);
        public Agent<TState>.Data MakeMove(Agent<TState>.Data newState);
    }
}