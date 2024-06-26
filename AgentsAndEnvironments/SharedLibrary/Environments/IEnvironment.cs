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
        public void RegisterAgent(StateToken<IState> currentStateToken, TState state);
        public List<TMovement> GetMovements(StateToken<IState> stateToken);
        public TResult MakeMove(TMovement movement);
    }
}