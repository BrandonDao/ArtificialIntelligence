using SharedLibrary.Agents;
using SharedLibrary.Movement;
using SharedLibrary.Movement.Results;
using SharedLibrary.States;

namespace SharedLibrary.Environments
{
    public interface IEnvironment<TState, TMovement, TResult>
        where TState : IState
        where TMovement : IMovement<TState, TResult>
        where TResult : IResult<TState>
    {
        public void RegisterAgent(IAgent<TState> agent, TState startingState, StateToken<IState> startingStateToken);
        public List<TMovement> GetMovements(IAgent<TState> agent);
        public List<TMovement> GetMovements(IAgent<TState> agent, StateToken<IState> currentStateToken);
        public TResult MakeMove(IAgent<TState> agent, TMovement movement);
    }
}