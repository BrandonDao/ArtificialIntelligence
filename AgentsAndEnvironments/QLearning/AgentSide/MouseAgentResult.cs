using SharedLibrary;
using SharedLibrary.Movement.Results;
using SharedLibrary.States;
using System.Diagnostics;

namespace QLearning.AgentSide
{
    [DebuggerDisplay("{SuccessorState}")]
    public struct MouseAgentResult(MouseState state, StateToken<IState> stateToken) : IResult<MouseState>
    {
        public MouseState State => state;
        public StateToken<IState> StateToken => stateToken;
    }
}