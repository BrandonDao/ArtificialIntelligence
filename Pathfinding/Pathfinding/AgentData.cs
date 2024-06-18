using Pathfinding.States;
using System.Diagnostics;

namespace Pathfinding
{
    [DebuggerDisplay("Wrapper: {Vertex.Value}")]
    public class AgentData<TState>(TState value, AgentData<TState>? founder, float priority, float distance)
        where TState : IState
    {
        public TState State { get; set; } = value;
        public AgentData<TState>? Predecessor { get; set; } = founder;
        public float Priority { get; set; } = priority;
        public float DistanceFromStart { get; set; } = distance;
    }
}