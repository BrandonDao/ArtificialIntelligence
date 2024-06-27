using SharedLibrary.Movement;

namespace QLearning.AgentSide
{
    public class MouseAgentMovement(MouseAgentResult[] results) : IMovement<MouseState, MouseAgentResult>
    {
        public MouseAgentResult[] Results { get; } = results;
    }
}