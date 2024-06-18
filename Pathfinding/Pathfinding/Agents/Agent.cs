using Pathfinding.Environments;
using Pathfinding.Frontiers;
using Pathfinding.States;

namespace Pathfinding.Agents
{
    public class Agent<TState>
        where TState : IState
    {
        public class Data(TState state, StateToken<IState> stateToken, Data? founder, float priority, float distance)
        {
            public TState State { get; } = state;
            public StateToken<IState> StateToken { get; } = stateToken;
            public Data? Predecessor { get; } = founder;
            public float Priority { get; } = priority;
            public float DistanceFromStart { get; } = distance;
        }

        private readonly IFrontier<TState> frontier;
        private readonly IEnvironment<TState> environment;
        private readonly Func<Data, HashSet<TState>, Edge<TState>, float> getPriority;

        private readonly HashSet<TState> visited;

        private Data currentData;

        public Agent(
            TState startingState,
            IFrontier<TState> frontier,
            IEnvironment<TState> environment,
            Func<Data, HashSet<TState>, Edge<TState>, float> getPriority)
        {
            visited = [];
            this.frontier = frontier;
            this.environment = environment;
            this.getPriority = getPriority;

            currentData = new Data(startingState, new StateToken<IState>(startingState), founder: null, priority: 0, distance: 0);

            environment.RegisterAgent(currentData.StateToken, currentData.State);

            frontier.Enqueue(currentData, currentData.Priority);
        }

        public bool MakeMove(Predicate<TState> hasReachedGoal)
        {
            currentData = frontier.Dequeue();
            visited.Add(currentData.State);

            List<Edge<TState>> edges = environment.GetSuccessors(currentData.StateToken);

            foreach (Edge<TState> edge in edges)
            {
                float newPriority = getPriority.Invoke(currentData, visited, edge);

                Data nextData = new(
                    state: edge.End,
                    stateToken: edge.EndStateToken,
                    founder: currentData,
                    priority: newPriority,
                    distance: currentData.DistanceFromStart + edge.Weight);

                nextData = environment.MakeMove(nextData);

                if (hasReachedGoal.Invoke(nextData.State))
                {
                    currentData = nextData;
                    return true;
                }

                frontier.Enqueue(nextData, newPriority);
            }

            return false;
        }

        public Data GetFinishedState() => currentData;
    }
}