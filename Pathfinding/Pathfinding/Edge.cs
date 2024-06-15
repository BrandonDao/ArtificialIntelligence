using System.Diagnostics;

namespace Pathfinding
{
    [DebuggerDisplay("{Start.Value} -> {End.Value}")]
    public class Edge<T>(T start, T end, float weight)
    {
        public T Start { get; set; } = start;
        public T End { get; set; } = end;
        public float Weight { get; set; } = weight;
    }
}