using SharedLibrary.Movement;
using static QLearning.AgentSide.MouseAgentMovement;

namespace QLearning.AgentSide
{
    public readonly struct MouseAgentMovement(Directions direction, MouseAgentResult[] results) : IMovement<MouseState, MouseAgentResult>
    {
        public enum Directions
        {
            Up,
            Down,
            Left,
            Right
        }
        public Directions Direction { get; } = direction;
        public MouseAgentResult[] Results { get; } = results;
    }
}