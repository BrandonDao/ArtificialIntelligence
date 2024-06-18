using Pathfinding.Environments;
using Pathfinding.Frontiers;
using Pathfinding.States;

namespace Pathfinding
{
    public class Agent<TState, TFrontier>
        where TState : IState
        where TFrontier : IFrontier<TState>, new()
    {
        private readonly IFrontier<TState> frontier;
        private readonly IEnvironment<TState> environment;
        private readonly Func<AgentData<TState>, HashSet<TState>, Edge<TState>, float> getPriority;

        private readonly HashSet<TState> visited;

        public Agent(TState startingState, IEnvironment<TState> environment, Func<AgentData<TState>, HashSet<TState>, Edge<TState>, float> getPriority)
        {
            frontier = new TFrontier();
            visited = [];
            this.getPriority = getPriority;
            this.environment = environment;
            frontier.Enqueue(new AgentData<TState>(startingState, founder: null, priority: 0, distance: 0), priority: 0);
        }

        public bool MakeMove(out AgentData<TState>? finalState)
        {
            finalState = null;

            AgentData<TState> currState = frontier.Dequeue();
            visited.Add(currState.State);

            List<Edge<TState>> children = environment.GetSuccessors(currState.State);

            foreach (Edge<TState> edge in children)
            {
                float newPriority = getPriority.Invoke(currState, visited, edge);

                AgentData<TState> nextStateData = new(
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