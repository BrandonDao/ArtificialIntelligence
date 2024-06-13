using System.Diagnostics;

namespace Pathfinding.Graph
{
    [DebuggerDisplay("{Start.Value} -> {End.Value}")]
    public class Edge<T>(Vertex<T> start, Vertex<T> end, float weight)
    {
        public Vertex<T> Start { get; set; } = start;
        public Vertex<T> End { get; set; } = end;
        public float Weight { get; set; } = weight;
    }
}