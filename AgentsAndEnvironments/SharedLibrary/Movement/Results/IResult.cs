using SharedLibrary.States;

namespace SharedLibrary.Movement.Results
{
    public interface IResult<TState>
        where TState : IState
    {
        public TState State { get; }
        public StateToken<IState> StateToken { get; }
    }
}