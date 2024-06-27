using SharedLibrary;
using SharedLibrary.Movement.Results;
using SharedLibrary.States;
using System.Diagnostics;

namespace QLearning.AgentSide
{
    [DebuggerDisplay("{SuccessorState}")]
    public class MouseAgentResult(MouseState state, StateToken<IState> stateToken) : IResult<MouseState>
    {
        public MouseState SuccessorState => state;
        public StateToken<IState> SuccessorStateToken => stateToken;
    }
}