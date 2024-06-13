using System.Diagnostics;
using Pathfinding.Graph;

namespace Pathfinding.Wrappers
{
    [DebuggerDisplay("Wrapper: {Vertex.Value}")]
    public class SearchState<T>(Vertex<T> vertex, SearchState<T>? founder, int priority, float distance)
    {
        public Vertex<T> Vertex { get; set; } = vertex;
        public SearchState<T>? Founder { get; set; } = founder;
        public int Priority { get; set; } = priority;
        public float DistanceFromStart { get; set; } = distance;
    }
}