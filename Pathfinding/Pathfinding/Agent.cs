using Pathfinding.Environments;
using Pathfinding.Frontiers;
using Pathfinding.States;
using System.Diagnostics;

namespace Pathfinding
{
    public static class Agent
    {
        [DebuggerDisplay("Wrapper: {Vertex.Value}")]
        public class Data<T>(T value, Data<T>? founder, float priority, float distance)
        {
            public T Value { get; set; } = value;
            public Data<T>? Predecessor { get; set; } = founder;
            public float Priority { get; set; } = priority;
            public float DistanceFromStart { get; set; } = distance;
        }

        public static Data<TState> FindPath<TState, TFrontier>(
                TState start,
                IEnvironment<TState> environment,
                Func<Data<TState>, HashSet<TState>, Edge<TState>, float> getPriority)
            where TState : IState
            where TFrontier : IFrontier<TState>, new()
        {
            IFrontier<TState> frontier = new TFrontier();
            frontier.Enqueue(new(start, founder: null, priority: 0, distance: 0), priority: 0);

            HashSet<TState> visited = [];

            while (frontier.Count > 0)
            {
                Data<TState> currState = frontier.Dequeue();
                visited.Add(currState.Value);

                List<Edge<TState>> children = environment.GetSuccessors(currState.Value);

                foreach (Edge<TState> edge in children)
                {
                    float newPriority = getPriority.Invoke(currState, visited, edge);

                    Data<TState> nextState = new(
                        value: edge.End,
                        founder: currState,
                        priority: newPriority,
                        distance: currState.DistanceFromStart + edge.Weight);

                    if (nextState.Value.Equals(environment.GoalState)) return nextState;

                    frontier.Enqueue(nextState, newPriority);
                }
            }

            throw new ArgumentException("Invalid search parameters!");
        }
    }
}