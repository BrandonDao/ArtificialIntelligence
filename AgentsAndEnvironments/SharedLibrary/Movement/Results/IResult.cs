using SharedLibrary.States;

namespace SharedLibrary.Movement.Results
{
    public interface IResult<TState>
        where TState : IState
    {
        public TState SuccessorState { get; }
        public StateToken<IState> SuccessorStateToken { get; }
    }
}