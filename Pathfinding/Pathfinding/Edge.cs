using Pathfinding.States;
using System.Diagnostics;

namespace Pathfinding
{
    [DebuggerDisplay("{Start.Value} -> {End.Value}")]
    public class Edge<TState>(TState start, TState end, float weight, float probability)
        where TState : IState
    {
        public TState Start { get;} = start;
        public TState End { get; } = end;
        public StateToken<IState> EndStateToken { get; } = new(end);
        public float Weight { get; } = weight;
        public float Probability { get; } = probability;
    }
}