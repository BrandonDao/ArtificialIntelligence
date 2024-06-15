using System.Diagnostics;

namespace Pathfinding
{
    [DebuggerDisplay("Wrapper: {Vertex.Value}")]
    public class AgentData<T>(T value, AgentData<T>? founder, float priority, float distance)
    {
        public T Value { get; set; } = value;
        public AgentData<T>? Predecessor { get; set; } = founder;
        public float Priority { get; set; } = priority;
        public float DistanceFromStart { get; set; } = distance;
    }
}