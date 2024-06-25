using SharedLibrary.Agents;
using SharedLibrary.States;

namespace SharedLibrary.Environments
{
    public interface IEnvironment<TState> where TState : IState
    {
        public TState GoalState { get; }
        public void RegisterAgent(StateToken<IState> currentStateToken, TState state);
        public List<Movement<TState>> GetMovements(StateToken<IState> stateToken);
        public AgentData<TState> MakeMove(AgentData<TState> newState);
    }
}