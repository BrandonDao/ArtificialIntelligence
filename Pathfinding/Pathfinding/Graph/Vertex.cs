using System.Diagnostics;

namespace Pathfinding.Graph
{
    [DebuggerDisplay("{Value}")]
    public class Vertex<T>(T value)
    {
        public T Value { get; set; } = value;
        public List<Edge<T>> Edges { get; set; } = [];

        public static bool operator ==(Vertex<T> lhs, Vertex<T> rhs) => lhs.Value!.Equals(rhs.Value);
        public static bool operator !=(Vertex<T> lhs, Vertex<T> rhs) => !(lhs == rhs);
        public override bool Equals(object? obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
    }
}