using Pathfinding.Environments;
using Pathfinding.Frontiers;
using Pathfinding.States;
using System.Diagnostics;

namespace Pathfinding
{
    public class Agent<TState>
        where TState : IState
    {
        [DebuggerDisplay("Wrapper: {Vertex.Value}")]
        public class AgentData(TState value, AgentData? founder, float priority, float distance)
        {
            public TState State { get; set; } = value;
            public AgentData? Predecessor { get; set; } = founder;
            public float Priority { get; set; } = priority;
            public float DistanceFromStart { get; set; } = distance;
        }

        private readonly IFrontier<TState> frontier;
        private readonly IEnvironment<TState> environment;
        private readonly Func<AgentData, HashSet<TState>, Edge<TState>, float> getPriority;

        private readonly HashSet<TState> visited;

        public Agent(
            TState startingState,
            IFrontier<TState> frontier,
            IEnvironment<TState> environment,
            Func<AgentData, HashSet<TState>, Edge<TState>, float> getPriority)
        {
            visited = [];
            this.frontier = frontier;
            this.environment = environment;
            this.getPriority = getPriority;
            frontier.Enqueue(new AgentData(startingState, founder: null, priority: 0, distance: 0), priority: 0);
        }

        public bool MakeMove(out AgentData? finalState)
        {
            finalState = null;

            AgentData currState = frontier.Dequeue();
            visited.Add(currState.State);

            List<Edge<TState>> children = environment.GetSuccessors(currState.State);

            foreach (Edge<TState> edge in children)
            {
                float newPriority = getPriority.Invoke(currState, visited, edge);

                AgentData nextStateData = new(
                    value: edge.End,
                    founder: currState,
                    priority: newPriority,
                    distance: currState.DistanceFromStart + edge.Weight);

                nextStateData = environment.MakeMove(nextStateData);

                if (nextStateData.State.Equals(environment.GoalState))
                {
                    finalState = nextStateData;
                    return true;
                }

                frontier.Enqueue(nextStateData, newPriority);
            }

            return false;
        }
    }
}