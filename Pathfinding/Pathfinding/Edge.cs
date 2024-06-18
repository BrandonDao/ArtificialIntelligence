using Pathfinding.States;
using System.Diagnostics;

namespace Pathfinding
{
    [DebuggerDisplay("{Start.Value} -> {End.Value}")]
    public class Edge<TState>(TState start, TState end, float weight)
        where TState : IState
    {
        public TState Start { get;} = start;
        public TState End { get; } = end;
        public StateToken<IState> EndStateToken { get; } = new(end);
        public float Weight { get; set; } = weight;
    }
}