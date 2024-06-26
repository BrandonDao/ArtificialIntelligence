using SharedLibrary.Movement.Results;
using SharedLibrary.States;

namespace SharedLibrary.Movement
{
    public interface IMovement<TState, TResult>
        where TState : IState
        where TResult : IResult<TState>
    {
        TResult[] Results { get; }
    }
}